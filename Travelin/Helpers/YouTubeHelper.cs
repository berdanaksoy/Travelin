namespace Travelin.Helpers
{
    public static class YouTubeHelper
    {
        public static string NormalizeUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return url;

            string videoId = null;

            if (url.Contains("watch?v="))
                videoId = url.Split("watch?v=")[1].Split('&')[0];
            else if (url.Contains("youtu.be/"))
                videoId = url.Split("youtu.be/")[1].Split('?')[0];
            else if (url.Contains("/embed/"))
                videoId = url.Split("/embed/")[1].Split('?')[0];

            if (string.IsNullOrWhiteSpace(videoId))
                return url;

            return $"https://www.youtube.com/embed/{videoId}?controls=1&rel=0";
        }
    }
}