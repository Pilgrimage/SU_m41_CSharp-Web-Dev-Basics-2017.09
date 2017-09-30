namespace p02_SocialNetwork.Data
{
    public static class TagTransformer
    {
        public static string Transform(string tag)
        {
            string modifTag = tag.Replace(" ", string.Empty);

            if (modifTag[0]!='#')
            {
                modifTag = "#" + modifTag;
            }

            if (modifTag.Length>20)
            {
                modifTag = modifTag.Substring(0, 20);
            }

            return modifTag;
        }
    }
}