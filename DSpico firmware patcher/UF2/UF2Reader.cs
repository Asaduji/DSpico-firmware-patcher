namespace DSpico_firmware_patcher.UF2
{
    public class UF2Reader
    {
        public List<UF2Block> Blocks = [];

        public void ReadFromBuffer(byte[] buffer)
        {
            using var ms = new MemoryStream(buffer);
            using var reader = new BinaryReader(ms);

            while (reader.BaseStream.Length - reader.BaseStream.Position >= 512)
            {
                var block = new UF2Block();
                block.Read(reader);
                Blocks.Add(block);
            }
        }

        public byte[] ToBuffer()
        {
            var buffer = new byte[512 * Blocks.Count];

            using var ms = new MemoryStream(buffer);
            using var writer = new BinaryWriter(ms);

            foreach (var block in Blocks)
            {
                block.Write(writer);
            }

            return buffer;
        }
    }
}
