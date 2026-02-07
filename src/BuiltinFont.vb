Public Enum BuiltinFont As Byte
  [Default] = 0
  PacArrange = 1
  NamcoClassic = 2
  Contra = 3
  SonicPocket = 4
  Xevious = 5
  Gradius3 = 6
  DigDug1996 = 7
  Flicky = 8
  KamikCabbie = 9
End Enum

Friend Module BuiltinFontExtensions
  <Runtime.CompilerServices.Extension>
  Friend Function GetFontSheet(font As BuiltinFont) As String
    With New Text.StringBuilder
      Select Case font
        Case BuiltinFont.Default
          ' Done: Default font sheet from the original `vbPixelGameEngine`
          .Append("?Q`0001oOch0o01o@F40o0<AGD4090LAGD<090@A7ch0?00O7Q`0600>00000000")
          .Append("O000000nOT0063Qo4d8>?7a14Gno94AA4gno94AaOT0>o3`oO400o7QN00000400")
          .Append("Of80001oOg<7O7moBGT7O7lABET024@aBEd714AiOdl717a_=TH013Q>00000000")
          .Append("720D000V?V5oB3Q_HdUoE7a9@DdDE4A9@DmoE4A;Hg]oM4Aj8S4D84@`00000000")
          .Append("OaPT1000Oa`^13P1@AI[?g`1@A=[OdAoHgljA4Ao?WlBA7l1710007l100000000")
          .Append("ObM6000oOfMV?3QoBDD`O7a0BDDH@5A0BDD<@5A0BGeVO5ao@CQR?5Po00000000")
          .Append("Oc``000?Ogij70PO2D]??0Ph2DUM@7i`2DTg@7lh2GUj?0TO0C1870T?00000000")
          .Append("70<4001o?P<7?1QoHg43O;`h@GT0@:@LB@d0>:@hN@L0@?aoN@<0O7ao0000?000")
          .Append("OcH0001SOglLA7mg24TnK7ln24US>0PL24U140PnOgl0>7QgOcH0K71S0000A000")
          .Append("00H00000@Dm1S007@DUSg00?OdTnH7YhOfTL<7Yh@Cl0700?@Ah0300700000000")
          .Append("<008001QL00ZA41a@6HnI<1i@FHLM81M@@0LG81?O`0nC?Y7?`0ZA7Y300080000")
          .Append("O`082000Oh0827mo6>Hn?Wmo?6HnMb11MP08@C11H`08@FP0@@0004@000000000")
          .Append("00P00001Oab00003OcKP0006@6=PMgl<@440MglH@000000`@000001P00000000")
          .Append("Ob@8@@00Ob@8@Ga13R@8Mga172@8?PAo3R@827QoOb@820@0O`0007`0000007P0")
          .Append("O`000P08Od400g`<3V=P0G`673IP0`@3>1`00P@6O`P00g`<O`000GP800000000")
          .Append("?P9PL020O`<`N3R0@E4HC7b0@ET<ATB0@@l6C4B0O`H3N7b0?P01L3R000000020")
        Case BuiltinFont.PacArrange
          ' Done: Add font sheet from Pac-Man Arrangement 1996
          .Append("73`0o01o8Wh0o01oBFH0o0MoEFH090\ACGh0900ADC`0?00O3P00600>00000000")
          .Append("O00063PnOT50?7aoOglL96Ao4gl?=6AA4gl7o3aAOT03o7aoO000P41n00000000")
          .Append("Of<0305oOg<?O7moOg\7N7moBGl014PABFl?14AaOfH737ao=P0063Q>00000000")
          .Append("76<R23PV?V]oG7a_Of\RG4a?Hf\RE4Ao@GlRM4aiHcioM0Qk8P0R800b00000000")
          .Append("Oah41303Oah^?gQ1Oa0ZOdQo@GmoOdUoHglZA7mo?WljI3m1700@840300000000")
          .Append("Ofm2?30oOflUO7QoOf\B@7aoBF\8H5A0BG\T?5A0@C=BO5ao000Q@1Po00000000")
          .Append("Oc`f70POOgio?7hoOfm9O7mo2F\i@7m`2G]FH0Th2C=8?0<O0@00708?00000000")
          .Append("76<001Qo?W<0?;AoOc\?O:@hHal7H:@LB@l0>?PhN@L0H7aoN@00O0Ao0000?000")
          .Append("OcH0001gOglLA7moOf\nK7ln26]SO1PL27m1>0PnOcH0>7QoO`00K71g0000A000")
          .Append("06H00007@Fm1C0@?Of]Sg3eoOg\nW7ehOclLT7eh@Ah0h40?0000O0070000?000")
          .Append("<008021QL01[I61aL6HnM>1mHFHLO81o@@0nO8AOO`1[G?e7?`08C7e300000000")
          .Append("O`000000O`0807moOkH827mo?7Hn=S11MP08@GQ1H`08@F`0@@0004@000000000")
          .Append("00P00001Oa@00002OaB`03l4Ob9`Ogl8@28007l@@000040P@000001000000000")
          .Append("O`00@GP0Oa@8@Ga13Q@8=PA171@827ao3Q@800AoOa@807`0O`0007P000000000")
          .Append("O`000W`0Ob800g`4OR9P0GP271AP0``1>1@00P@2O`P00g`4O`000GP000000000")
          .Append("?PI0L3R0O`LPB7b0Oe<@A7b0@ET8@TB0@El4A4B0O`l2B7b0?PH1L3R000000020")
        Case BuiltinFont.NamcoClassic
          ' Done: Classic Namco style font sheet
          .Append("?Q`0001oOch0o01o@F40o0LAFD4090\AED<0900A7ch0?00O7Q`0600>00000000")
          .Append("O00063PnOT00?7ao4d8794A14GmO93aA4gmOo7aaOT00o40oO400P01N00000000")
          .Append("Of80105oOg<;O7moBGT7N7lABET034PaBEd;14QiOdl737Q_=TH0231>00000000")
          .Append("720D03PV?V5o26a_HdUoG4A9@DdDE4A9@DmoE6a;Hg]oM2Qj8S4D800`00000000")
          .Append("OaPV1000Oaa_13P1@AI9?g`1@A=oOdAoHgm;A3mo?WmjI7l1710`840100000000")
          .Append("ObM6000oOfLU?3QoBDDCO7a0BDD8@5A0BDET@5A0@GeB?5ao03PaO0Po00000000")
          .Append("Oc`f000?Ogi?70PO2D]9?7hh2DU9@7m`2DTm@0Th2GUl?0<O0C18708?00000000")
          .Append("70<005Qo?P<0?:AoHg4;O:@h@GT7@;@LB@d0>;PhN@L0@5aoN@<0O0Ao0000?000")
          .Append("OcH0001SOdlL07mg24dnA7ln25USK0PL25U1>0PnOgH0>7QgOc00K71S0000A000")
          .Append("00H00000@Dm1C007@DUSg0@?OdTnT7ehOfTLT7eh@Cl0o00?@Ah0O00700000000")
          .Append("<008001QL00ZA41a@6HLI<1i@6IoM81M@00LG81?O`0ZC?e7?`08A7e300000000")
          .Append("O`080000O`0827mo6;Hn?Wmo?7HnMc11MP08@GQ1H`08@F`0@@0004@000000000")
          .Append("00P00001Oa`00002OcJ`0044@6=`Ogl8@44007l@@000000P@000001000000000")
          .Append("Ob@8@G`0Ob@8@Ga13R@8M`a172@8?WQo3R@820aoOb@807`0O`0007P000000000")
          .Append("O`000P04Od400g`63V=P0GP373IP0`@3>1`00P@6O`P00g`4O`000GP000000000")
          .Append("?P90L020O`<PN3R0@E4@C4B0@ET8ATB0@@T4C4B0O`l2N7b0?PH1L3R000000020")
        Case BuiltinFont.Contra
          ' Done: Add font sheet from Contra / Time Pilot '84
          .Append("73`0`01P8Wh0i01iBD<0?0LOEDT090\CCF40900ADCl0?00K7Qh0600>00000000")
          .Append("L000430h6010>7Qn5F8L;4a34g8>96AAOgh4i3@aO`<0n7Qo@040W4a>00000000")
          .Append("@600061PNG8;I3UiCgT7O5l?BDT024\IBDd;14AiOdl733a_=PH061P600000000")
          .Append(">20R2310O49jG7PVCTTWG4a?@dTRE4A=@GUbM4AIHCL_M2aj9PHR80Pa00000000")
          .Append("H1PT1200NA@Z=G1QCg9oOdQm@GTZCfPOHamoA3T3?Q<Z17l17Q0B04L100000000")
          .Append("H216830aNFiUM7QmOd\CG5aOCd\8@5A3BD]T85@`BG]CO1`<0C4aG0P300000000")
          .Append("H3P`10P1NGaFO4Qo?di?O7io3dXY@7lP2DUa80TH2GU>70440C48308300000000")
          .Append(">0@00403O68019Qo@W<;O;@H@@\7@:@?:@L0>>AhN@<0@7PL6P40L0@30000?000")
          .Append("H300061075HL@74Q2dlnI1lGJ5eW70lnO5U3>0Qd7cL1L7120`H0C40100001000")
          .Append("@4@00000@4i00003LD]QQ3AWObUcW7enAcTnd4dH0AlLH2040@h0?00300003000")
          .Append("H008061P@@0Z@<1cHFHLI81I>FH8M81=3`0LG>A70`0ZC7eS0@0811d100000000")
          .Append("L0000000O008061`3kH8275o6gHn?Qm??008McM1HP08@FP1@@0004`0000000@0")
          .Append("H0000000N@P00003Cab`0344@cI`Ogl8@28004l@@000041PH000000000000000")
          .Append("H000@@@061@8Mga03a@8?PA1N1@827Qa31@800AoOa@807P?@000040000000000")
          .Append("L00040@0300027`47b9P37P6O3IP70`1L1`060@670P027P40`00140000000000")
          .Append(">010H300OP1PF7R0@d8@A4b0@A48@TB0H@T4A4B0?`l3F3b03PH1H1R000000000")
        Case BuiltinFont.SonicPocket
          ' Done: Add font sheet from Sonic Pocket Adventure
          .Append("?Sh0O01o@Gl0O01oGDT0O0=oEDD050LACGl070PODGl0700O7Sh0200>00000000")
          .Append("H000220n744077Ao4gl075Ao7gm_55A9Ogm_O7aoO41_O7`oH000O7Qn00000000")
          .Append("OgT0O7moOgd7O7moOgd3O7moADD024@9OdL737aoOdL337ao>T8033Qf00000000")
          .Append("?T4TF3Q6OdEnG7a?OdDTG7a?@DDBE4A=@GloM4Am@GlBM4Am@CX0=4@i00000000")
          .Append("OaP\13P1Oa@Z17`1Oa8Z?gao@GmoOdAoOglZOgaoOglZA7l1?Q0JA7l100000000")
          .Append("OdM7?3PoOdLUO7aoOdLGO7aoADD8@5A0AGedO5aoAGeBO5ao@CUaO5Po00000000")
          .Append("Och`70@OOgm:?0@oOgm5O7mo1DE5@7m01Gd[O7mo1Gd@?0Do0CU\70DO00000000")
          .Append("?P40?4QoO`40O5aoOgT7@5ao@Gd3?5@HNGl0@7aoN@L0O7ahN@<0?3ao00000000")
          .Append("OcX0C7m3OglL;7lWOglR77lO14E160@nOgm1L7alOgl0J7abOcX0I7QQ00000000")
          .Append("04h0C00705m1G00?Oem1G7eoOe4RD7e`OelLG7eo07l0O00?03h0?00700000000")
          .Append("@000I41a@00RM41i@6`DO41m@6aoO7eOO`0DO7e?O`0RG3e7?`00C01300000000")
          .Append("O`0807l0O`0827moOgH8=Wm113Io@A11OP08@Ga1N`0806`0L@0806`000000000")
          .Append("O`P00001Oa@00042Ob:`07l4@45`Mgl8@000Mgl@@000000P@000001000000000")
          .Append("Ob8807`03b88@Ga1?b88@@A13288=Wa1Ob8820AoO`0807`0O`0007P000000000")
          .Append("O`0027`83d4017`47b9P17`2?QAP20@1O0P047`2N00047`4O`0027P800000000")
          .Append("?P9P<3R0O`<`>7b0OeTH?7b0@ET<?TB0Oel6?7b0O`l3>7b0?PH1<3R000000020")
        Case BuiltinFont.Xevious
          ' Done: Add font sheet from Xevious (by Namco)
          .Append("03h0001o?7l0o01o@Wl0o0=oFT40A0LAFT40A0@AG440?00A03h0000>00000000")
          .Append("OP0>000n4@5OO7Ao4GmOO5Ao4Gl?A5A1Ogl6a7a1O`00o7aaOP00000n00000000")
          .Append("Og40001oOgT7O7l?OgT7O4L?BDT024@9BDl717aiBDl717ai=TH0001f00000000")
          .Append("?R80000VOd4DG7a?OdTnG7a?@DTDE4A9@GlnM4Ai@GlDM4Ai@CH0000b00000000")
          .Append("Oc`40001@B8^17`1@B4^14Ao@Gm[O4AoOgljOgmoOglj17l1?R0@000100000000")
          .Append("Odm2000oOdlUO7aoOdlBO5AoBDT8@5A0BGTT@5a0BGUBO5a0BC4Q000o00000000")
          .Append("Och`000?Odmj70@OOdmm?7lo2DU?H7e`2GUG80D`2GTR70@@2C5@000?00000000")
          .Append("?P<0O00o@@44O=aoBG47@;a0BGd3O:AoNgl0O?aoN`l0@>A0>P<0O00o00000000")
          .Append("OcH00013OdlnC7lWOdmo?0LO24U140@L27U1N7al27T0I7abOcH0001Q00000000")
          .Append("04H0000704m1O00?Odm1O7eoOdUo@7ehOgTn`01h07T0o00803h0000700000000")
          .Append("0000001Q@00ZI81a@3HLM81iOcH8O;eMO`0LG?e??`0ZC0170000001300000000")
          .Append("O`0000003d0807mo3gH821mo63Hn?Q11O008MgQ1LP08@F@0H@00000000000000")
          .Append("O`000001O`Q00002OaA`07l4@28`Ogl8@000000@@000000P@000001000000000")
          .Append("O`00@G`00A@8M`A1Oa@8?Wa1Oa@827ao0A@800AoOa@807`0OP0007`000000000")
          .Append("O`000P000@000G`40B9`0@@20AA`0P@1O`P00W`2O`000W`4OP000@0000000000")
          .Append("?P90L020O`4PB7b0Oe4@A4B0@ET8@TB0@@l4A7b0@@l2B7b0?PH1L02000000020")
        Case BuiltinFont.Gradius3
          ' Done: Add font sheet from Gradius 3 (by Konami)
          .Append("?Sh0001m@F40o009GE4090L9EDT090\9CDD09009DD<090097Sh0600600000000")
          .Append("H000000n<00063Q1:09@94A197l<94A18P0?94AA8@0392@QO`00o7aN00000400")
          .Append("OG44001mBDT3O7l9BDT324@9BDT014@9BDT414@9BDT314@I=TH313QV00000000")
          .Append("744D00168T5dB3Q9@DTOE4A9@DTDE4A9@DUlE4A9@DTGE4A9@CHD94@a00000000")
          .Append("OC0T1001@BPZ13P1@B@Z14@1@B9oOdAo@B4Z14@18WTZ14@1720@07l100000000")
          .Append("74M2000O:TTU?3PPBDTB@5A0BDT8@5A0BDTT@5A0BDUB@5@PBC4QO5PO00000000")
          .Append("O3hf000o2TU970Q02DU980PP2DU9@7h@2DTm@0T82DU880T42C5870T300000000")
          .Append("7040001o8T44?0PH@B43@9@6@A43@9AoB@T0>9@`B@D0@9@<<@<0@7`30000?000")
          .Append("OcH0001124TLA7lR24TRA0PD24U1:0P824U140PD24T0:0PROcH0A7110000A000")
          .Append("04H0000304U1S00404U1T0@8OdTRT3e`04TLT40804T0D00403h0?00300000000")
          .Append("00080011@00ZA41Q@6<LI81A@6<8E819@00LE815800ZC8A37`08A7e100000000")
          .Append("O`002000000827mo2;<8=P1127<n@A115008@A118P08@BP0@@0004@000000000")
          .Append("7`P0000181@00002@2:`0044@45`Mcl8@000040@@000040P@000001000000000")
          .Append("O`00@@001Q@8@Ga121@8@@A141@8=PA121@827Qo1Q@820@0Oa@800@0000007P0")
          .Append("O`0020080T4017`4129P10P221AP30@140P020@2800020@4O`0017P800000000")
          .Append("?P50L020@@4PB3R0@E4@A4B0@@T8@TB0@@T4A4B0@@T2B4B0?PH1L3R000000020")
        Case BuiltinFont.DigDug1996
          ' ToDo: Add font sheet from Dig-Dug Arrangement 1996
          .Append("")
        Case BuiltinFont.Flicky
          ' ToDo: Add font sheet from Flicky (by SEGA)
          .Append("")
        Case BuiltinFont.KamikCabbie
          ' ToDo: Add font sheet from Kamikaze Cabbie (by Data East)
          .Append("")
        Case Else
          Throw New ArgumentException($"Unknown built-in font name: {font}", NameOf(font))
      End Select
      Return .ToString()
    End With
  End Function
End Module
