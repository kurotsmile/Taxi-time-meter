// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("vujMc3kJLxZ8kSt3QxQ2xRNtSGvFd/TXxfjz/N9zvXMC+PT09PD19trAtQEY58Q3gZyTyZEaZCCE+m1cd/T69cV39P/3d/T09XsRvMjjOXZHYVHIIwD+Bw8vu8syqoeQY/YeqqgVPUjMF1g7NBpQagArIQiPYG/brbix6TUtatG2W3rs0gw0hfHddCzVWLccCBlZ5EPt/DsoxZD+wy+uDpjmLnbBe1+POTzTgdLXWw4toqGdKO9iuhUmf65Ifu9v1hgn06VoyU8m/qwx7H089qjLbOq/0k3/AOTUFOBG6EmRcyEAnAwPLadc25i1oomBhBDi+qwiqbuoDe7Bst6qEA7jsj1pxjYxFfrWjGdUls52Kh9Vt4CA9eMvxKuKykC5kvf29PX0");
        private static int[] order = new int[] { 12,12,9,12,10,10,6,11,10,9,10,11,12,13,14 };
        private static int key = 245;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
