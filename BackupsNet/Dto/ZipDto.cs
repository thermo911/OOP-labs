using System.IO;

namespace BackupsNet.Dto
{
    public class ZipDto
    {
        public string DestinationFolder { get; set; }
        public byte[] Data { get; set; }
    }
}