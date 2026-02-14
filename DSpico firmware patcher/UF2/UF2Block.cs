namespace DSpico_firmware_patcher.UF2
{
    public class UF2Block
    {
        public uint FirstMagic;
        public uint SecondMagic;
        public uint Flags;
        public uint Address;
        public uint DataSize;
        public uint BlockNumber;
        public uint TotalBlocks;
        public uint FileSize;
        public byte[] Data = [];
        public uint FinalMagicNumber;

        public void Read(BinaryReader reader)
        {
            FirstMagic = reader.ReadUInt32();
            SecondMagic = reader.ReadUInt32();
            Flags = reader.ReadUInt32();
            Address = reader.ReadUInt32();
            DataSize = reader.ReadUInt32();
            BlockNumber = reader.ReadUInt32();
            TotalBlocks = reader.ReadUInt32();
            FileSize = reader.ReadUInt32();
            Data = reader.ReadBytes(476);
            FinalMagicNumber = reader.ReadUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(FirstMagic);
            writer.Write(SecondMagic);
            writer.Write(Flags);
            writer.Write(Address);
            writer.Write(DataSize);
            writer.Write(BlockNumber);
            writer.Write(TotalBlocks);
            writer.Write(FileSize);
            writer.Write(Data);
            writer.Write(FinalMagicNumber);
        }
    }
}
