namespace DSpico_firmware_patcher.UF2
{
    public class UF2Patcher
    {
        private readonly UF2Reader _uf2Reader;
        private readonly byte[] _dummyPattern = "DUMMYBEGIN"u8.ToArray();
        public UF2Patcher(UF2Reader uf2Reader)
        {
            _uf2Reader = uf2Reader;
        }

        public void PatchWRFU(byte[] wrfuRom)
        {
            var currentBlockOffset = 0;
            var currentBlock = _uf2Reader.Blocks[0];

            // Find dummy pattern
            for (var i = 0; i < _uf2Reader.Blocks.Count; i++)
            {
                currentBlock = _uf2Reader.Blocks[i];

                currentBlockOffset = GetBlockPatternOffset(currentBlock);

                if (currentBlockOffset >= 0)
                {
                    break;
                }
            }

            if (currentBlockOffset == -1)
            {
                throw new Exception("Could not find dummy pattern");
            }
            
            var wrfuOffset = 0;

            //Now start replacing dummy with WRFU
            while (wrfuOffset < wrfuRom.Length)
            {
                if (currentBlockOffset >= currentBlock.DataSize)
                {
                    currentBlock = _uf2Reader.Blocks.First(x => x.Address == currentBlock.Address + currentBlock.DataSize);
                    currentBlockOffset = 0;
                }

                currentBlock.Data[currentBlockOffset] = wrfuRom[wrfuOffset];

                ++currentBlockOffset;
                ++wrfuOffset;
            }
        }

        private int GetBlockPatternOffset(UF2Block block)
        {
            for (var i = 0; i < block.DataSize; i++)
            {
                for (var j = 0; j < _dummyPattern.Length; j++)
                {
                    if (block.Data[i + j] != _dummyPattern[j])
                    {
                        break;
                    }

                    // Pattern could be split between 2 blocks, we need to take that into account
                    // Check next block if pattern is matching but we're at the end of the block
                    if (j == _dummyPattern.Length - 1)
                    {
                        var nextBlock = _uf2Reader.Blocks.FirstOrDefault(x => x.Address == block.Address + block.DataSize);

                        if (nextBlock is not null && BlockStartsWithPattern(nextBlock, j))
                        {
                            return i;
                        }

                    }

                    if (j >= _dummyPattern.Length - 1)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        private bool BlockStartsWithPattern(UF2Block block, int patternOffset)
        {
            if (patternOffset >= _dummyPattern.Length)
            {
                return false;
            }

            var remaining = _dummyPattern.Length - patternOffset;

            if (block.DataSize < remaining)
            {
                return false;
            }

            for (var i = patternOffset; i < _dummyPattern.Length; i++)
            {
                if (block.Data[i] != _dummyPattern[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
