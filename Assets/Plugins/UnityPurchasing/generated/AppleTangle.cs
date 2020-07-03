#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("blU0Z0B7vWqi719JIDuoaqwYoaoYHXEbSRogGyItKH4vLTgpfngaOL61USePbKBw/z0cGODvJGblP0L6C2hqG6kqCRsmLSIBrWOt3CYqKiqrPwD7Qmy/XSLV30CmBWuN3GxmVKAyovXSYEfeLIAJGynDMxXTeyL4G6kvkBupKIiLKCkqKSkqKRsmLSJCTUJISl9CREULal5fQ0RZQl9SGltHTgt5RERfC2hqGzU8JhsdGx8ZURupKl0bJS0ofjYkKirULy8oKSpfQk1CSEpfTgtJUgtKRVILW0pZX1lKSF9CSE4LWF9KX05GTkVfWAUb4jJZ3nYl/lR0sNkOKJF+pGZ2JtpbR04LaE5ZX0JNQkhKX0JERQtqXpXfWLDF+U8k4FJkH/OJFdJT1EDjFg1MC6EYQdwmqeT1wIgE0nhBcE9FTwtIREVPQl9CREVYC0RNC15YTiS2FtgAYgMx49XlnpIl8nU3/eAW8h1U6qx+8oyykhlp0PP+WrVVinkNGw8tKH4vIDg2altbR04LaE5ZX4P3VQke4Q7+8iT9QP+JDwg63IqHqSorLSIBrWOt3EhPLiobqtkbAS00uvA1bHvALsZ1Uq8GwB2JfGd+xyzHVhKooHgL+BPvmpSxZCFA1ADXTx4IPmA+cjaYv9zdt7Xke5Hqc3ukWKpL7TBwIgS5mdNvY9tLE7U+3oCIWrlseH7qhARqmNPQyFvmzYhnHhkaHxsYHXE8JhgeGxkbEhkaHxsBrWOt3CYqKi4uKxtJGiAbIi0ofkdOC2JFSAUaDRsPLSh+LyA4NmpbBBuq6C0jAC0qLi4sKSkbqp0xqphyjC4iVzxrfTo1X/icoAgQbIj+RC0ofjYlLz0vPwD7Qmy/XSLV30CmD8nA+pxb9CRuygzh2kZTxsyePDwtGyQtKH42OCoq1C8uGygqKtQbNgtKRU8LSE5ZX0JNQkhKX0JERQtbHbJnBlOcxqew99hcsNld+VwbZOrrSBhc3BEsB33A8SQKJfGRWDJknl9DRFlCX1IaPRs/LSh+Lyg4JmpbLy04KX54GjgbOi0ofi8hOCFqW1sFa43cbGZUI3UbNC0ofjYILzMbPWLzXbQYP06KXL/iBikoKisqiKkqGzotKH4vITghaltbR04LYkVIBRojdRupKjotKH42Cy+pKiMbqSovGzSuqK4wshZsHNmCsGulB/+auznzVGqDs9L64U23D0A6+4iQzzAB6DRMpCOfC9zghwcLRFudFCobp5xo5AtETQtfQ04LX0NORQtKW1tHQkhKeU5HQkpFSE4LREULX0NCWAtITlmaG3PHcS8Zp0OYpDb1TljUTHVOlyYtIgGtY63cJioqLi4rKKkqKit3LisoqSokKxupKiEpqSoqK8+6giJSC0pYWF5GTlgLSkhITltfSkVITpwwlrhpDzkB7CQ2nWa3dUjjYKs8XFwFSltbR04FSERGBEpbW0dOSEo9Gz8tKH4vKDgmaltbR04LeUREXwcLSE5ZX0JNQkhKX04LW0RHQkhSSUdOC1hfSkVPSllPC19OWUZYC0qeEYbfJCUruSCaCj0FX/4XJvBJPSMALSouLiwpKj01Q19fW1gRBARce4Gh/vHP1/siLBybXl4K");
        private static int[] order = new int[] { 25,8,18,13,32,16,25,11,25,28,39,46,19,32,50,56,43,22,58,39,54,34,56,51,54,55,54,46,55,32,37,52,37,53,37,45,48,57,43,58,52,46,55,53,49,56,57,57,53,54,57,57,52,59,59,58,59,58,58,59,60 };
        private static int key = 43;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
