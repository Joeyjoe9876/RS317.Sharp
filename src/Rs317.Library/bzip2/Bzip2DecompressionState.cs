
//From Major at Rune-Server
//https://www.rune-server.ee/runescape-development/rs2-client/snippets/430099-bzip2-classes-refactor.html

/// <summary>
/// A Java implementation of the DState struct (structure holding all the
/// decompression-side stuff).
/// 
/// @see http 
///      ://svn.apache.org/repos/asf/labs/axmake/trunk/src/libuc++/srclib/bzip2
///      /bzlib_private.h
/// </summary>
public class BZip2DecompressionState
{
	// Class32

	/* Constants */
	int bzMaxAlphaSize = 258;
	int bzMaxCodeLen = 23;
	int bzRunB = 1;
	int bzNGroups = 6;
	int bzGSize = 50;
	int bzNIters = 4;
	int bzMaxSelectors = 18002; // (2 + (900000 / BZ_G_SIZE))

	/*-- Constants for the fast MTF decoder. --*/
	int mtfaSize = 4096;
	int mtflSize = 16;

	/* for undoing the Burrows-Wheeler transform (FAST) */
	public static int[] tt;

	/* map of bytes used in block */
	int nInUse;
	bool[] inUse;
	bool[] inUse16;
	byte[] seqToUnseq;

	byte[] stream;
	byte[] buf; // out

	/* for doing the run-length decoding */
	int stateOutLen;
	bool blockRandomised;
	byte stateOutCh;

	/* the buffer for bit stream reading */
	int bsBuff;
	int bsLive;

	/* misc administratium */
	int blockSize100k;
	int currBlockNumber;

	/* for undoing the Burrows-Wheeler transform */
	int origPtr;
	int tPos;
	int k0;
	int nBlockUsed;
	int[] unzftab;
	int[] cftab;

	/* for decoding the MTF values */
	byte[] mtfa;
	int[] mtfbase;
	byte[] selector;
	byte[] selectorMtf;
	byte[][] len;

	int[] minLens;
	int[][] limit;
	int[][] base;
	int[][] perm;

	int nBlock;
	int nextIn;
	int availableIn;
	int totalInLo32;
	int totalInHi32;
	int nextOut;
	int availOut;
	int totalOutLo32;
	int totalOutHigh32;

	BZip2DecompressionState()
	{
		unzftab = new int[256];
		cftab = new int[257];
		inUse = new bool[256];
		inUse16 = new bool[16];
		seqToUnseq = new byte[256];
		mtfa = new byte[4096];
		mtfbase = new int[16];
		selector = new byte[18002];
		selectorMtf = new byte[18002];
		len = new byte[6][258];
		limit = new int[6][258];
		base = new int[6][258];
		perm = new int[6][258];
		minLens = new int[6];
	}

}