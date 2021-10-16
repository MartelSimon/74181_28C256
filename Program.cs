using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _28C256_74181_ALU
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] eeprom_data = new byte[32768];
            for (uint i = 0; i < eeprom_data.Length; i++)
            {
                if ((i & 0b_0001_0000) == 16) // If M Enable
                {
                    uint a = ((i >> 5) & 0b_1111); //A Input
                    uint b = (i >> 9) & 0b_1111; //B Input

                    switch ((i & 0b_0000_1111)) // Switch Selection
                    {
                        case 0: // If Selection is 0 [Not A]
                            {
                                eeprom_data[i] = (byte)((~a) & 0b_1111);
                                continue;
                            }
                        case 1: // If Selection is 1 [NAND A B]
                            {
                                uint c = a & b;
                                eeprom_data[i] = (byte)((~c) & 0b_1111);
                                continue;
                            }
                        case 2: // If Selection is 2 [OR Not A B]
                            {
                                uint c = ((~a) & 0b_1111) | b;
                                eeprom_data[i] = (byte)c;
                                continue;
                            }
                        case 3: // If Selection is 3 [Logical 1]
                            {
                                eeprom_data[i] = 15;
                                continue;
                            }
                        case 4: // If Selection is 4 [NOR A B]
                            {
                                uint c = a | b;
                                eeprom_data[i] = (byte)((~c) & 0b_1111);
                                continue;
                            }
                        case 5: // If Selection is 5 [Not B]
                            {
                                eeprom_data[i] = (byte)((~b) & 0b_1111);
                                continue;
                            }
                        case 6: // If Selection is 6 [XNOR A B]
                            {
                                uint c = a ^ b;
                                eeprom_data[i] = (byte)((~c) & 0b_1111);
                                continue;
                            }
                        case 7: // If Selection is 7 [OR A Not B]
                            {
                                uint c = a | ((~b) & 0b_1111);
                                eeprom_data[i] = (byte)c;
                                continue;
                            }
                        case 8: // If Selection is 8 [AND Not A B]
                            {
                                uint c = ((~a) & 0b_1111) & b;
                                eeprom_data[i] = (byte)c;
                                continue;
                            }
                        case 9: // If Selection is 9 [XOR A B]
                            {
                                uint c = a ^ b;
                                eeprom_data[i] = (byte)c;
                                continue;
                            }
                        case 10: // If Selection is 10 [B]
                            {
                                uint c = a ^ b;
                                eeprom_data[i] = (byte)c;
                                continue;
                            }
                        case 11: // If Selection is 11 [OR A B]
                            {
                                uint c = a | b;
                                eeprom_data[i] = (byte)c;
                                continue;
                            }
                        case 12: // If Selection is 12 [Logical 0]
                            {
                                eeprom_data[i] = 0;
                                continue;
                            }
                        case 13: // If Selection is 13 [AND A Not B]
                            {
                                uint c = a & ((~b) & 0b_1111);
                                eeprom_data[i] = (byte)c;
                                continue;
                            }
                        case 14: // If Selection is 14 [AND A B]
                            {
                                uint c = a & b;
                                eeprom_data[i] = (byte)c;
                                continue;
                            }
                        case 15: // If Selection is 15 [A]
                            {
                                eeprom_data[i] = (byte)a;
                                continue;
                            }
                    }
                }
                else
                {
                    uint a = ((i >> 5) & 0b_1111); //A Input
                    uint b = (i >> 9) & 0b_1111; //B Input
                    uint c = ((i >> 14) & 0b_0001); // Carry Input
                    uint ac = a + c; //A + Carry
                    switch ((i & 0b_0000_1111)) // Switch Selection
                    {
                        case 0: // If Selection is 0 [A minus 1]
                            {
                                eeprom_data[i] = (byte)((ac - 1) & 0b_11111);
                                continue;
                            }
                        case 1: // If Selection is 1 [AND A B minus 1]
                            {
                                uint d = a & b;
                                eeprom_data[i] = (byte)(((d - 1) + c) & 0b_11111);
                                continue;
                            }
                        case 2: // If Selection is 2 [AND A Not B minus 1]
                            {
                                uint d = a & ((~b) & 0b_1111);
                                eeprom_data[i] = (byte)(((d - 1) + c) & 0b_11111);
                                continue;
                            }
                        case 3: // If Selection is 3 [-1]
                            {
                                eeprom_data[i] = (byte)((-1 + c) & 0b_11111);
                                continue;
                            }
                        case 4: // If Selection is 4 [A plus OR A Not B]
                            {
                                uint d = a + (a | ((~b) & 0b_1111));
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 5: // If Selection is 5 [A AND B plus OR A Not B]
                            {
                                uint d = (a & b) + (a | ((~b) & 0b_1111));
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 6: // If Selection is 6 [A minus B minus 1]
                            {
                                uint d = (a - b) - 1;
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 7: // If Selection is 7 [A plus Not B]
                            {
                                uint d = a + ((~b) & 0b_1111);
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 8: // If Selection is 8 [A plus OR A B]
                            {
                                uint d = a + (a | b);
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 9: // If Selection is 9 [A plus B]
                            {
                                uint d = a + b;
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 10: // If Selection is 10 [AND A Not B plus OR A B]
                            {
                                uint d = (a & ((~b) & 0b_1111)) + (a | b);
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 11: // If Selection is 11 [A Or B]
                            {
                                uint d = a | b;
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 12: // If Selection is 12 [A plus A]
                            {
                                uint d = a + a;
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 13: // If Selection is 13 [AND A B plus A]
                            {
                                uint d = (a & b) + a;
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 14: // If Selection is 14 [AND A Not B plus A]
                            {
                                uint d = (a & ((~b) & 0b_1111)) + a;
                                eeprom_data[i] = (byte)((d + c) & 0b_11111);
                                continue;
                            }
                        case 15: // If Selection is 15 [A]
                            {
                                eeprom_data[i] = (byte)((ac) & 0b_11111);
                                continue;
                            }
                    }
                }
                eeprom_data[i] = 0;
            }
            File.WriteAllBytes("a.out", eeprom_data);
        }
    }
}
