using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NVorbis.jorbis
{
class Residue0 : FuncResidue
{
	static Object StatickLock = new Object();
  override internal void pack(Object vr, NVorbis.jogg.Buffer opb){
    InfoResidue0 info=(InfoResidue0)vr;
    int acc=0;
    opb.write(info.begin, 24);
    opb.write(info.end, 24);

    opb.write(info.grouping-1, 24); /* residue vectors to group and 
          			     code with a partitioned book */
    opb.write(info.partitions-1, 6); /* possible partition choices */
    opb.write(info.groupbook, 8); /* group huffman book */

    /* secondstages is a bitmask; as encoding progresses pass by pass, a
       bitmask of one indicates this partition class has bits to write
       this pass */
    for(int j=0; j<info.partitions; j++){
      int i=info.secondstages[j];
      if(Util.ilog(i)>3){
        /* yes, this is a minor hack due to not thinking ahead */
        opb.write(i, 3);
        opb.write(1, 1);
        opb.write((int)(((uint)i)>>3), 5);
      }
      else{
        opb.write(i, 4); /* trailing zero */
      }
      acc+=Util.icount(i);
    }
    for(int j=0; j<acc; j++){
      opb.write(info.booklist[j], 8);
    }
  }

  override internal Object unpack(Info vi, NVorbis.jogg.Buffer opb)
  {
    int acc=0;
    InfoResidue0 info=new InfoResidue0();
    info.begin=opb.read(24);
    info.end=opb.read(24);
    info.grouping=opb.read(24)+1;
    info.partitions=opb.read(6)+1;
    info.groupbook=opb.read(8);

    for(int j=0; j<info.partitions; j++){
      int cascade=opb.read(3);
      if(opb.read(1)!=0){
        cascade|=(opb.read(5)<<3);
      }
      info.secondstages[j]=cascade;
      acc+=Util.icount(cascade);
    }

    for(int j=0; j<acc; j++){
      info.booklist[j]=opb.read(8);
    }

    if(info.groupbook>=vi.books){
      free_info(info);
      return (null);
    }

    for(int j=0; j<acc; j++){
      if(info.booklist[j]>=vi.books){
        free_info(info);
        return (null);
      }
    }
    return (info);
  }

  override internal Object look(DspState vd, InfoMode vm, Object vr)
  {
    InfoResidue0 info=(InfoResidue0)vr;
    LookResidue0 look=new LookResidue0();
    int acc=0;
    int dim;
    int maxstage=0;
    look.info=info;
    look.map=vm.mapping;

    look.parts=info.partitions;
    look.fullbooks=vd.fullbooks;
    look.phrasebook=vd.fullbooks[info.groupbook];

    dim=look.phrasebook.dim;

    look.partbooks=new int[look.parts][];

    for(int j=0; j<look.parts; j++){
      int i=info.secondstages[j];
      int stages=Util.ilog(i);
      if(stages!=0){
        if(stages>maxstage)
          maxstage=stages;
        look.partbooks[j]=new int[stages];
        for(int k=0; k<stages; k++){
          if((i&(1<<k))!=0){
            look.partbooks[j][k]=info.booklist[acc++];
          }
        }
      }
    }

    look.partvals=(int)Math.Round(Math.Pow(look.parts, dim));
    look.stages=maxstage;
    look.decodemap=new int[look.partvals][];
    for(int j=0; j<look.partvals; j++){
      int val=j;
      int mult=look.partvals/look.parts;
      look.decodemap[j]=new int[dim];

      for(int k=0; k<dim; k++){
        int deco=val/mult;
        val-=deco*mult;
        mult/=look.parts;
        look.decodemap[j][k]=deco;
      }
    }
    return (look);
  }

  override internal void free_info(Object i)
  {
  }

  override internal void free_look(Object i)
  {
  }

  private static int[][][] _01inverse_partword=new int[2][][]; // _01inverse is synchronized for

  // re-using partword
  static internal int _01inverse(Block vb, Object vl, float[][] In, int ch, int decodepart)
  {
	  lock (StatickLock) {
    int i, j, k, l, s;
    LookResidue0 look=(LookResidue0)vl;
    InfoResidue0 info=look.info;

    // move all this setup out later
    int samples_per_partition=info.grouping;
    int partitions_per_word=look.phrasebook.dim;
    int n=info.end-info.begin;

    int partvals=n/samples_per_partition;
    int partwords=(partvals+partitions_per_word-1)/partitions_per_word;

    if(_01inverse_partword.Length<ch){
      _01inverse_partword=new int[ch][][];
    }

    for(j=0; j<ch; j++){
      if(_01inverse_partword[j]==null||_01inverse_partword[j].Length<partwords){
        _01inverse_partword[j]=new int[partwords][];
      }
    }

    for(s=0; s<look.stages; s++){
      // each loop decodes on partition codeword containing 
      // partitions_pre_word partitions
      for(i=0, l=0; i<partvals; l++){
        if(s==0){
          // fetch the partition word for each channel
          for(j=0; j<ch; j++){
            int temp=look.phrasebook.decode(vb.opb);
            if(temp==-1){
              return (0);
            }
            _01inverse_partword[j][l]=look.decodemap[temp];
            if(_01inverse_partword[j][l]==null){
              return (0);
            }
          }
        }

        // now we decode residual values for the partitions
        for(k=0; k<partitions_per_word&&i<partvals; k++, i++)
          for(j=0; j<ch; j++){
            int offset=info.begin+i*samples_per_partition;
            int index=_01inverse_partword[j][l][k];
            if((info.secondstages[index]&(1<<s))!=0){
              CodeBook stagebook=look.fullbooks[look.partbooks[index][s]];
              if(stagebook!=null){
                if(decodepart==0){
                  if(stagebook.decodevs_add(In[j], offset, vb.opb,
                      samples_per_partition)==-1){
                    return (0);
                  }
                }
                else if(decodepart==1){
					if (stagebook.decodev_add(In[j], offset, vb.opb,
                      samples_per_partition)==-1){
                    return (0);
                  }
                }
              }
            }
          }
      }
    }
    return (0);
  }
  }

  static internal int[][] _2inverse_partword = null;

  static internal int _2inverse(Block vb, Object vl, float[][] In, int ch){
	  lock (StatickLock) {
    int i, k, l, s;
    LookResidue0 look=(LookResidue0)vl;
    InfoResidue0 info=look.info;

    // move all this setup out later
    int samples_per_partition=info.grouping;
    int partitions_per_word=look.phrasebook.dim;
    int n=info.end-info.begin;

    int partvals=n/samples_per_partition;
    int partwords=(partvals+partitions_per_word-1)/partitions_per_word;

    if(_2inverse_partword==null||_2inverse_partword.Length<partwords){
      _2inverse_partword=new int[partwords][];
    }
    for(s=0; s<look.stages; s++){
      for(i=0, l=0; i<partvals; l++){
        if(s==0){
          // fetch the partition word for each channel
          int temp=look.phrasebook.decode(vb.opb);
          if(temp==-1){
            return (0);
          }
          _2inverse_partword[l]=look.decodemap[temp];
          if(_2inverse_partword[l]==null){
            return (0);
          }
        }

        // now we decode residual values for the partitions
        for(k=0; k<partitions_per_word&&i<partvals; k++, i++){
          int offset=info.begin+i*samples_per_partition;
          int index=_2inverse_partword[l][k];
          if((info.secondstages[index]&(1<<s))!=0){
            CodeBook stagebook=look.fullbooks[look.partbooks[index][s]];
            if(stagebook!=null){
              if(stagebook.decodevv_add(In, offset, ch, vb.opb,
                  samples_per_partition)==-1){
                return (0);
              }
            }
          }
        }
      }
    }
    return (0);
  }
  }

  override internal int inverse(Block vb, Object vl, float[][] In, int[] nonzero, int ch)
  {
    int used=0;
    for(int i=0; i<ch; i++){
      if(nonzero[i]!=0){
        In[used++]=In[i];
      }
    }
    if(used!=0)
		return (_01inverse(vb, vl, In, used, 0));
    else
      return (0);
  }

  internal class LookResidue0{
	  internal InfoResidue0 info;
	  internal int map;

	  internal int parts;
	  internal int stages;
	  internal CodeBook[] fullbooks;
	  internal CodeBook phrasebook;
	  internal int[][] partbooks;

	  internal int partvals;
	  internal int[][] decodemap;

	  internal int postbits;
	  internal int phrasebits;
	  internal int frames;
  }

  internal class InfoResidue0
  {
    // block-partitioned VQ coded straight residue
	  internal int begin;
	  internal int end;

    // first stage (lossless partitioning)
	  internal int grouping; // group n vectors per partition
	  internal int partitions; // possible codebooks for a partition
	  internal int groupbook; // huffbook for partitioning
	  internal int[] secondstages = new int[64]; // expanded out to pointers in lookup
	  internal int[] booklist = new int[256]; // list of second stage books

    // encode-only heuristic settings
	  internal float[] entmax = new float[64]; // book entropy threshholds
	  internal float[] ampmax = new float[64]; // book amp threshholds
	  internal int[] subgrp = new int[64]; // book heuristic subgroup size
	  internal int[] blimit = new int[64]; // subgroup position limits
  }

}

}
