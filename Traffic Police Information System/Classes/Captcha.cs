namespace Traffic_Police_Information_System.Classes
{
    public class Captcha
    {
        string image;
        string kod;

        public Captcha(string _image, string _kod) 
        { 
            image= _image;
            kod = _kod;
        }

        public bool CheckKod(string _kod)
        {
            if (kod == _kod)
            { 
                return true; 
            }
            return false;
        }

        public string GetImage()
        {
            return image;
        }

        public string GetKod()
        {
            return kod;
        }
    }
}
