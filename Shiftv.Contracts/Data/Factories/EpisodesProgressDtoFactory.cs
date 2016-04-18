using System.Collections.Generic;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.Factories
{
    public class EpisodesProgressDtoFactory
    {
        public static List<int> Create(EpisodesProgressDto dto, bool IsSeen)
        {

            var listToSee = new List<int>();
            var listSeen = new List<int>();

            if (dto.Episode1 != null && dto.Episode1.Value)
            {
                listSeen.Add(1);
            }
            if (dto.Episode1 != null && !dto.Episode1.Value)
            {
                listToSee.Add(1);
            }

            if (dto.Episode2 != null && dto.Episode2.Value)
            {
                listSeen.Add(2);
            }
            if (dto.Episode2 != null && !dto.Episode2.Value)
            {
                listToSee.Add(2);
            }

            if (dto.Episode3 != null && dto.Episode3.Value)
            {
                listSeen.Add(3);
            }
            if (dto.Episode3 != null && !dto.Episode3.Value)
            {
                listToSee.Add(3);
            }

            if (dto.Episode4 != null && dto.Episode4.Value)
            {
                listSeen.Add(4);
            }
            if (dto.Episode4 != null && !dto.Episode4.Value)
            {
                listToSee.Add(4);
            }

            if (dto.Episode5 != null && dto.Episode5.Value)
            {
                listSeen.Add(5);
            }
            if (dto.Episode5 != null && !dto.Episode5.Value)
            {
                listToSee.Add(5);
            }


            if (dto.Episode6 != null && dto.Episode6.Value)
            {
                listSeen.Add(6);
            }
            if (dto.Episode6 != null && !dto.Episode6.Value)
            {
                listToSee.Add(6);
            }

            if (dto.Episode7 != null && dto.Episode7.Value)
            {
                listSeen.Add(7);
            }
            if (dto.Episode7 != null && !dto.Episode7.Value)
            {
                listToSee.Add(7);
            }

            if (dto.Episode8 != null && dto.Episode8.Value)
            {
                listSeen.Add(8);
            }
            if (dto.Episode8 != null && !dto.Episode8.Value)
            {
                listToSee.Add(8);
            }

            if (dto.Episode9 != null && dto.Episode9.Value)
            {
                listSeen.Add(9);
            }
            if (dto.Episode9 != null && !dto.Episode9.Value)
            {
                listToSee.Add(9);
            }

            if (dto.Episode10 != null && dto.Episode10.Value)
            {
                listSeen.Add(10);
            }
            if (dto.Episode10 != null && !dto.Episode10.Value)
            {
                listToSee.Add(10);
            }

            if (dto.Episode11 != null && dto.Episode11.Value)
            {
                listSeen.Add(11);
            }
            if (dto.Episode11 != null && !dto.Episode11.Value)
            {
                listToSee.Add(11);
            }

            if (dto.Episode12 != null && dto.Episode12.Value)
            {
                listSeen.Add(12);
            }
            if (dto.Episode12 != null && !dto.Episode12.Value)
            {
                listToSee.Add(12);
            }

            if (dto.Episode13 != null && dto.Episode13.Value)
            {
                listSeen.Add(13);
            }
            if (dto.Episode13 != null && !dto.Episode13.Value)
            {
                listToSee.Add(13);
            }

            if (dto.Episode14 != null && dto.Episode14.Value)
            {
                listSeen.Add(14);
            }
            if (dto.Episode14 != null && !dto.Episode14.Value)
            {
                listToSee.Add(14);
            }


            if (dto.Episode15 != null && dto.Episode15.Value)
            {
                listSeen.Add(15);
            }
            if (dto.Episode15 != null && !dto.Episode15.Value)
            {
                listToSee.Add(15);
            }


            if (dto.Episode16 != null && dto.Episode16.Value)
            {
                listSeen.Add(16);
            }
            if (dto.Episode16 != null && !dto.Episode16.Value)
            {
                listToSee.Add(16);
            }


            if (dto.Episode17 != null && dto.Episode17.Value)
            {
                listSeen.Add(17);
            }
            if (dto.Episode17 != null && !dto.Episode17.Value)
            {
                listToSee.Add(17);
            }


            if (dto.Episode18 != null && dto.Episode18.Value)
            {
                listSeen.Add(18);
            }
            if (dto.Episode18 != null && !dto.Episode18.Value)
            {
                listToSee.Add(18);
            }

            if (dto.Episode19 != null && dto.Episode19.Value)
            {
                listSeen.Add(19);
            }
            if (dto.Episode19 != null && !dto.Episode19.Value)
            {
                listToSee.Add(19);
            }

            if (dto.Episode20 != null && dto.Episode20.Value)
            {
                listSeen.Add(20);
            }
            if (dto.Episode20 != null && !dto.Episode20.Value)
            {
                listToSee.Add(20);
            }

            if (dto.Episode21 != null && dto.Episode21.Value)
            {
                listSeen.Add(21);
            }
            if (dto.Episode21 != null && !dto.Episode21.Value)
            {
                listToSee.Add(21);
            }

            if (dto.Episode22 != null && dto.Episode22.Value)
            {
                listSeen.Add(22);
            }
            if (dto.Episode22 != null && !dto.Episode22.Value)
            {
                listToSee.Add(22);
            }

            if (dto.Episode23 != null && dto.Episode23.Value)
            {
                listSeen.Add(23);
            }
            if (dto.Episode23 != null && !dto.Episode23.Value)
            {
                listToSee.Add(23);
            }

            if (dto.Episode24 != null && dto.Episode24.Value)
            {
                listSeen.Add(24);
            }
            if (dto.Episode24 != null && !dto.Episode24.Value)
            {
                listToSee.Add(24);
            }

            if (dto.Episode25 != null && dto.Episode25.Value)
            {
                listSeen.Add(25);
            }
            if (dto.Episode25 != null && !dto.Episode25.Value)
            {
                listToSee.Add(25);
            }

            if (dto.Episode26 != null && dto.Episode26.Value)
            {
                listSeen.Add(26);
            }
            if (dto.Episode26 != null && !dto.Episode26.Value)
            {
                listToSee.Add(26);
            }

            if (dto.Episode27 != null && dto.Episode27.Value)
            {
                listSeen.Add(27);
            }
            if (dto.Episode27 != null && !dto.Episode27.Value)
            {
                listToSee.Add(27);
            }

            if (dto.Episode28 != null && dto.Episode28.Value)
            {
                listSeen.Add(28);
            }
            if (dto.Episode28 != null && !dto.Episode28.Value)
            {
                listToSee.Add(28);
            }

            if (dto.Episode29 != null && dto.Episode29.Value)
            {
                listSeen.Add(29);
            }
            if (dto.Episode29 != null && !dto.Episode29.Value)
            {
                listToSee.Add(29);
            }

            if (dto.Episode30 != null && dto.Episode30.Value)
            {
                listSeen.Add(30);
            }
            if (dto.Episode30 != null && !dto.Episode30.Value)
            {
                listToSee.Add(30);
            }

            if (dto.Episode31 != null && dto.Episode31.Value)
            {
                listSeen.Add(31);
            }
            if (dto.Episode31 != null && !dto.Episode31.Value)
            {
                listToSee.Add(31);
            }

            if (dto.Episode32 != null && dto.Episode32.Value)
            {
                listSeen.Add(32);
            }
            if (dto.Episode32 != null && !dto.Episode32.Value)
            {
                listToSee.Add(32);
            }

            if (dto.Episode33 != null && dto.Episode33.Value)
            {
                listSeen.Add(33);
            }
            if (dto.Episode33 != null && !dto.Episode33.Value)
            {
                listToSee.Add(33);
            }

            if (dto.Episode34 != null && dto.Episode34.Value)
            {
                listSeen.Add(34);
            }
            if (dto.Episode34 != null && !dto.Episode34.Value)
            {
                listToSee.Add(34);
            }

            if (dto.Episode35 != null && dto.Episode35.Value)
            {
                listSeen.Add(35);
            }
            if (dto.Episode35 != null && !dto.Episode35.Value)
            {
                listToSee.Add(35);
            }

            if (dto.Episode36 != null && dto.Episode36.Value)
            {
                listSeen.Add(36);
            }
            if (dto.Episode36 != null && !dto.Episode36.Value)
            {
                listToSee.Add(36);
            }

            if (dto.Episode37 != null && dto.Episode37.Value)
            {
                listSeen.Add(37);
            }
            if (dto.Episode37 != null && !dto.Episode37.Value)
            {
                listToSee.Add(37);
            }

            if (dto.Episode38 != null && dto.Episode38.Value)
            {
                listSeen.Add(38);
            }
            if (dto.Episode38 != null && !dto.Episode38.Value)
            {
                listToSee.Add(38);
            }

            if (dto.Episode39 != null && dto.Episode39.Value)
            {
                listSeen.Add(39);
            }
            if (dto.Episode39 != null && !dto.Episode39.Value)
            {
                listToSee.Add(39);
            }

            if (dto.Episode40 != null && dto.Episode40.Value)
            {
                listSeen.Add(40);
            }
            if (dto.Episode40 != null && !dto.Episode40.Value)
            {
                listToSee.Add(40);
            }

            if (dto.Episode41 != null && dto.Episode41.Value)
            {
                listSeen.Add(41);
            }
            if (dto.Episode41 != null && !dto.Episode41.Value)
            {
                listToSee.Add(41);
            }

            if (dto.Episode42 != null && dto.Episode42.Value)
            {
                listSeen.Add(42);
            }
            if (dto.Episode42 != null && !dto.Episode42.Value)
            {
                listToSee.Add(42);
            }

            if (dto.Episode43 != null && dto.Episode43.Value)
            {
                listSeen.Add(43);
            }
            if (dto.Episode43 != null && !dto.Episode43.Value)
            {
                listToSee.Add(43);
            }

            if (dto.Episode44 != null && dto.Episode44.Value)
            {
                listSeen.Add(44);
            }
            if (dto.Episode44 != null && !dto.Episode44.Value)
            {
                listToSee.Add(44);
            }

            if (dto.Episode45 != null && dto.Episode45.Value)
            {
                listSeen.Add(45);
            }
            if (dto.Episode45 != null && !dto.Episode45.Value)
            {
                listToSee.Add(45);
            }

            if (dto.Episode46 != null && dto.Episode46.Value)
            {
                listSeen.Add(46);
            }
            if (dto.Episode46 != null && !dto.Episode46.Value)
            {
                listToSee.Add(46);
            }

            if (dto.Episode47 != null && dto.Episode47.Value)
            {
                listSeen.Add(47);
            }
            if (dto.Episode47 != null && !dto.Episode47.Value)
            {
                listToSee.Add(47);
            }

            if (dto.Episode48 != null && dto.Episode48.Value)
            {
                listSeen.Add(48);
            }
            if (dto.Episode48 != null && !dto.Episode48.Value)
            {
                listToSee.Add(48);
            }

            if (dto.Episode49 != null && dto.Episode49.Value)
            {
                listSeen.Add(49);
            }
            if (dto.Episode49 != null && !dto.Episode49.Value)
            {
                listToSee.Add(49);
            }

            if (dto.Episode50 != null && dto.Episode50.Value)
            {
                listSeen.Add(50);
            }
            if (dto.Episode50 != null && !dto.Episode50.Value)
            {
                listToSee.Add(50);
            }

            if (dto.Episode51 != null && dto.Episode51.Value)
            {
                listSeen.Add(51);
            }
            if (dto.Episode51 != null && !dto.Episode51.Value)
            {
                listToSee.Add(51);
            }

            if (dto.Episode52 != null && dto.Episode52.Value)
            {
                listSeen.Add(52);
            }
            if (dto.Episode52 != null && !dto.Episode52.Value)
            {
                listToSee.Add(52);
            }

            if (dto.Episode53 != null && dto.Episode53.Value)
            {
                listSeen.Add(53);
            }
            if (dto.Episode53 != null && !dto.Episode53.Value)
            {
                listToSee.Add(53);
            }

            if (dto.Episode54 != null && dto.Episode54.Value)
            {
                listSeen.Add(54);
            }
            if (dto.Episode54 != null && !dto.Episode54.Value)
            {
                listToSee.Add(54);
            }

            if (dto.Episode55 != null && dto.Episode55.Value)
            {
                listSeen.Add(55);
            }
            if (dto.Episode55 != null && !dto.Episode55.Value)
            {
                listToSee.Add(55);
            }

            if (dto.Episode56 != null && dto.Episode56.Value)
            {
                listSeen.Add(56);
            }
            if (dto.Episode56 != null && !dto.Episode56.Value)
            {
                listToSee.Add(56);
            }

            if (dto.Episode57 != null && dto.Episode57.Value)
            {
                listSeen.Add(57);
            }
            if (dto.Episode57 != null && !dto.Episode57.Value)
            {
                listToSee.Add(57);
            }


            if (dto.Episode58 != null && dto.Episode58.Value)
            {
                listSeen.Add(58);
            }
            if (dto.Episode58 != null && !dto.Episode58.Value)
            {
                listToSee.Add(58);
            }

            if (dto.Episode59 != null && dto.Episode59.Value)
            {
                listSeen.Add(59);
            }
            if (dto.Episode59 != null && !dto.Episode59.Value)
            {
                listToSee.Add(59);
            }
            if (dto.Episode59 != null && dto.Episode59.Value)
            {
                listSeen.Add(59);
            }
            if (dto.Episode59 != null && !dto.Episode59.Value)
            {
                listToSee.Add(59);
            }

            if (dto.Episode60 != null && dto.Episode60.Value)
            {
                listSeen.Add(60);
            }
            if (dto.Episode60 != null && !dto.Episode60.Value)
            {
                listToSee.Add(60);
            }

            if (dto.Episode61 != null && dto.Episode61.Value)
            {
                listSeen.Add(61);
            }
            if (dto.Episode61 != null && !dto.Episode61.Value)
            {
                listToSee.Add(61);
            }

            if (dto.Episode62 != null && dto.Episode62.Value)
            {
                listSeen.Add(62);
            }
            if (dto.Episode62 != null && !dto.Episode62.Value)
            {
                listToSee.Add(62);
            }

            if (dto.Episode63 != null && dto.Episode63.Value)
            {
                listSeen.Add(63);
            }
            if (dto.Episode63 != null && !dto.Episode63.Value)
            {
                listToSee.Add(63);
            }


            if (dto.Episode64 != null && dto.Episode64.Value)
            {
                listSeen.Add(64);
            }
            if (dto.Episode64 != null && !dto.Episode64.Value)
            {
                listToSee.Add(64);
            }

            if (dto.Episode65 != null && dto.Episode65.Value)
            {
                listSeen.Add(65);
            }
            if (dto.Episode65 != null && !dto.Episode65.Value)
            {
                listToSee.Add(65);
            }

            if (dto.Episode66 != null && dto.Episode66.Value)
            {
                listSeen.Add(66);
            }
            if (dto.Episode66 != null && !dto.Episode66.Value)
            {
                listToSee.Add(66);
            }

            if (dto.Episode67 != null && dto.Episode67.Value)
            {
                listSeen.Add(67);
            }
            if (dto.Episode67 != null && !dto.Episode67.Value)
            {
                listToSee.Add(67);
            }

            if (dto.Episode68 != null && dto.Episode68.Value)
            {
                listSeen.Add(68);
            }
            if (dto.Episode68 != null && !dto.Episode68.Value)
            {
                listToSee.Add(68);
            }

            if (dto.Episode69 != null && dto.Episode69.Value)
            {
                listSeen.Add(69);
            }
            if (dto.Episode69 != null && !dto.Episode69.Value)
            {
                listToSee.Add(69);
            }

            if (dto.Episode70 != null && dto.Episode70.Value)
            {
                listSeen.Add(70);
            }
            if (dto.Episode70 != null && !dto.Episode70.Value)
            {
                listToSee.Add(70);
            }

            if (dto.Episode71 != null && dto.Episode71.Value)
            {
                listSeen.Add(71);
            }
            if (dto.Episode71 != null && !dto.Episode71.Value)
            {
                listToSee.Add(71);
            }

            if (dto.Episode72 != null && dto.Episode72.Value)
            {
                listSeen.Add(72);
            }
            if (dto.Episode72 != null && !dto.Episode72.Value)
            {
                listToSee.Add(72);
            }

            if (dto.Episode73 != null && dto.Episode73.Value)
            {
                listSeen.Add(73);
            }
            if (dto.Episode73 != null && !dto.Episode73.Value)
            {
                listToSee.Add(73);
            }

            if (dto.Episode74 != null && dto.Episode74.Value)
            {
                listSeen.Add(74);
            }
            if (dto.Episode74 != null && !dto.Episode74.Value)
            {
                listToSee.Add(74);
            }

            if (dto.Episode75 != null && dto.Episode75.Value)
            {
                listSeen.Add(75);
            }
            if (dto.Episode75 != null && !dto.Episode75.Value)
            {
                listToSee.Add(75);
            }

            if (dto.Episode76 != null && dto.Episode76.Value)
            {
                listSeen.Add(76);
            }
            if (dto.Episode76 != null && !dto.Episode76.Value)
            {
                listToSee.Add(76);
            }

            if (dto.Episode77 != null && dto.Episode77.Value)
            {
                listSeen.Add(77);
            }
            if (dto.Episode77 != null && !dto.Episode77.Value)
            {
                listToSee.Add(77);
            }

            if (dto.Episode78 != null && dto.Episode78.Value)
            {
                listSeen.Add(78);
            }
            if (dto.Episode78 != null && !dto.Episode78.Value)
            {
                listToSee.Add(78);
            }

            if (dto.Episode79 != null && dto.Episode79.Value)
            {
                listSeen.Add(79);
            }
            if (dto.Episode79 != null && !dto.Episode79.Value)
            {
                listToSee.Add(79);
            }

            if (dto.Episode80 != null && dto.Episode80.Value)
            {
                listSeen.Add(80);
            }
            if (dto.Episode80 != null && !dto.Episode80.Value)
            {
                listToSee.Add(80);
            }

            if (dto.Episode81 != null && dto.Episode81.Value)
            {
                listSeen.Add(81);
            }
            if (dto.Episode81 != null && !dto.Episode81.Value)
            {
                listToSee.Add(81);
            }

            if (dto.Episode82 != null && dto.Episode82.Value)
            {
                listSeen.Add(82);
            }
            if (dto.Episode82 != null && !dto.Episode82.Value)
            {
                listToSee.Add(82);
            }

            if (dto.Episode83 != null && dto.Episode83.Value)
            {
                listSeen.Add(83);
            }
            if (dto.Episode83 != null && !dto.Episode83.Value)
            {
                listToSee.Add(83);
            }

            if (dto.Episode84 != null && dto.Episode84.Value)
            {
                listSeen.Add(84);
            }
            if (dto.Episode84 != null && !dto.Episode84.Value)
            {
                listToSee.Add(84);
            }

            if (dto.Episode85 != null && dto.Episode85.Value)
            {
                listSeen.Add(85);
            }
            if (dto.Episode85 != null && !dto.Episode85.Value)
            {
                listToSee.Add(85);
            }

            if (dto.Episode86 != null && dto.Episode86.Value)
            {
                listSeen.Add(86);
            }
            if (dto.Episode86 != null && !dto.Episode86.Value)
            {
                listToSee.Add(86);
            }


            if (dto.Episode87 != null && dto.Episode87.Value)
            {
                listSeen.Add(87);
            }
            if (dto.Episode87 != null && !dto.Episode87.Value)
            {
                listToSee.Add(87);
            }

            if (dto.Episode88 != null && dto.Episode88.Value)
            {
                listSeen.Add(88);
            }
            if (dto.Episode88 != null && !dto.Episode88.Value)
            {
                listToSee.Add(88);
            }

            if (dto.Episode89 != null && dto.Episode89.Value)
            {
                listSeen.Add(89);
            }
            if (dto.Episode89 != null && !dto.Episode89.Value)
            {
                listToSee.Add(89);
            }

            if (dto.Episode90 != null && dto.Episode90.Value)
            {
                listSeen.Add(90);
            }
            if (dto.Episode90 != null && !dto.Episode90.Value)
            {
                listToSee.Add(90);
            }

            if (dto.Episode91 != null && dto.Episode91.Value)
            {
                listSeen.Add(91);
            }
            if (dto.Episode91 != null && !dto.Episode91.Value)
            {
                listToSee.Add(91);
            }

            if (dto.Episode92 != null && dto.Episode92.Value)
            {
                listSeen.Add(92);
            }
            if (dto.Episode92 != null && !dto.Episode92.Value)
            {
                listToSee.Add(92);
            }

            if (dto.Episode93 != null && dto.Episode93.Value)
            {
                listSeen.Add(93);
            }
            if (dto.Episode93 != null && !dto.Episode93.Value)
            {
                listToSee.Add(93);
            }

            if (dto.Episode94 != null && dto.Episode94.Value)
            {
                listSeen.Add(94);
            }
            if (dto.Episode94 != null && !dto.Episode94.Value)
            {
                listToSee.Add(94);
            }

            if (dto.Episode95 != null && dto.Episode95.Value)
            {
                listSeen.Add(95);
            }
            if (dto.Episode95 != null && !dto.Episode95.Value)
            {
                listToSee.Add(95);
            }


            if (dto.Episode96 != null && dto.Episode96.Value)
            {
                listSeen.Add(96);
            }
            if (dto.Episode96 != null && !dto.Episode96.Value)
            {
                listToSee.Add(96);
            }

            if (dto.Episode97 != null && dto.Episode97.Value)
            {
                listSeen.Add(97);
            }
            if (dto.Episode97 != null && !dto.Episode97.Value)
            {
                listToSee.Add(97);
            }

            if (dto.Episode98 != null && dto.Episode98.Value)
            {
                listSeen.Add(98);
            }
            if (dto.Episode98 != null && !dto.Episode98.Value)
            {
                listToSee.Add(98);
            }

            if (dto.Episode99 != null && dto.Episode99.Value)
            {
                listSeen.Add(99);
            }
            if (dto.Episode99 != null && !dto.Episode99.Value)
            {
                listToSee.Add(99);
            }

            if (dto.Episode100 != null && dto.Episode100.Value)
            {
                listSeen.Add(100);
            }
            if (dto.Episode100 != null && !dto.Episode100.Value)
            {
                listToSee.Add(100);
            }




            if (IsSeen) return listSeen;
            return listToSee;

        }
    }
}