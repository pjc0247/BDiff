using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;


public class BHash
{
#if BHASH_USING_MEMCMP
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int memcmp(byte[] b1, byte[] b2, long count);
#endif
        public readonly static int BlockSize = 1024;

        public long FileSize { get; private set; }
        public List<byte[]> Blocks { get; private set; }

        internal BHash()
        {
            FileSize = -1;
            Blocks = new List<byte[]>();
        }

        public static BHash Load(byte[] data)
        {
            var bhash = new BHash();
            var mem = new MemoryStream(data);
            var cvt = new BinaryFormatter();
            
            bhash.FileSize = (long)cvt.Deserialize(mem);
            bhash.Blocks = (List<byte[]>)cvt.Deserialize(mem);

            return bhash;
        }
        public static BHash Load(String path)
        {
            var bhash = new BHash();
            var buffer = new byte[BlockSize];

            using (var md5 = MD5CryptoServiceProvider.Create())
            using (var fp = File.OpenRead(path))
            {
                bhash.FileSize = fp.Length;

                while (fp.Position < fp.Length)
                {
                    var read = fp.Read(buffer, 0, buffer.Length);

                    bhash.Blocks.Add(md5.ComputeHash(buffer));
                }
            }

            return bhash;
        }

        public byte[] Save()
        {
            var mem = new MemoryStream();
            var cvt = new BinaryFormatter();

            cvt.Serialize(mem, FileSize);
            cvt.Serialize(mem, Blocks);
            mem.Flush();

            return mem.ToArray();
        }

        private bool IsSameBlock(byte[] block1, byte[] block2)
        {
#if BHASH_USING_MEMCMP
            if (memcmp(block1, block2, block1.Length) != 0)
                return false;
#else
            if (!block1.SequenceEqual(block2))
                return false;
#endif   
            return true;   
        }

        /// <summary>
        /// 두 BHash가 가리키는 파일이 같은지 검사한다.
        /// </summary>
        /// <param name="other">검사할 다른 파일</param>
        /// <returns>같으면 true, 다른 부분이 있을 경우 false</returns>
        public bool IsSame(BHash other)
        {
            if (other.FileSize != FileSize)
                return false;
            if (FindFirstDiffPoint(other) != -1)
                return false;

            return true;
        }

        /// <summary>
        /// 두 BHash가 가리키는 파일에서 차이점들을 찾는다.
        /// </summary>
        /// <param name="other">검사할 다른 파일</param>
        /// <returns>차이점들의 offset이 담긴 List</returns>
        public List<long> FindDiff(BHash other)
        {
            var diffs = new List<long>();

            for (var i = 0; i < Blocks.Count; i++)
            {
                if (!IsSameBlock(Blocks[i], other.Blocks[i]))
                    diffs.Add(i * BlockSize);
            }

            return diffs;
        }

        /// <summary>
        /// 두 BHash가 가리키는 파일에서 첫 번째 차이점이 발생하는 지점을 찾는다.
        /// </summary>
        /// <param name="other">검사할 다른 파일</param>
        /// <returns>첫 번째 차이점 offset</returns>
        public long FindFirstDiffPoint(BHash other)
        {
            for (var i = 0; i < Blocks.Count; i++)
            {
                if (!IsSameBlock(Blocks[i], other.Blocks[i]))
                    return i * BlockSize;
            }
            return -1;
        }

        public static bool IsSame(String path1, String path2)
        {
            return (BHash.Load(path1)).IsSame(BHash.Load(path2));
        }
        public static List<long> FindDiff(String path1, String path2)
        {
            return (BHash.Load(path1)).FindDiff(BHash.Load(path2));
        }
        public static long FindFirstDiffPoint(String path1, String path2)
        {
            return (BHash.Load(path1)).FindFirstDiffPoint(BHash.Load(path2));
        }
}
