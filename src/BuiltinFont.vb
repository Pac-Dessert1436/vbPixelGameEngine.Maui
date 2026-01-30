Public Enum BuiltinFont As Byte
  [Default] = 0
  PacArrange = 1
  NamcoClassic = 2
  Contra = 3
  RollThunder = 4
  Xevious = 5
  Guardian = 6
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
          .Append("73`0o01o8Wh0o01oBFH0o0MoEFH090\ACGh0900A4C`0?00O3P00600>00000000")
          .Append("O01063PnOT4L?7aoOgl?96Ao4gl7=6AA4gl3o3aAOT00o7aoO000P41n00000000")
          .Append("Of<0105oOg<?O7moOg\7N7moBGl014PABFl?14AaOfH737ao=P0063Q>00000000")
          .Append("76<R23PV?V]oG7a_Of\RG4a?Hf\RE4Ao@GlRM4aiHcioM0Qk8P0R800b00000000")
          .Append("Oah41303Oah^?gQ1Oa0ZOdQo@GmoOdUoHglZA7mo?WljI3m1700@840300000000")
          .Append("Ofm2?30oOflUO7QoOf\B@7aoBF\8H5A0BG\T?5A0@C=BO5ao000Q@1Po00000000")
          .Append("Oc`f70POOgio?7hoOfm9O7mo2F\i@7m`2G]FH0Th2C=8?0<O0@00708?00000000")
          .Append("76<001Qo?W<0?;AoHc\?O:@h@Al7H:@LB@l0>?PhN@L0H7aoN@00O0Ao0000?000")
          .Append("OcH0001gOglLA7mo26\nK7ln26]SO1PL27m1>0PnOcH0>7QoO`00K71g0000A000")
          .Append("06H00007@Fm1C0@?Of]Sg3eoOg\nW7ehOclLT7eh@Ah0d40?0000O0070000?000")
          .Append("<008021QL01[I61aL6HnM>1mHFHLO81o@@0nO8AOO`1[G?e7?`08C7e300000000")
          .Append("O`000000O`0807moOkH827mo?7Hn=S11MP08@GQ1H`08@F`0@@0004@000000000")
          .Append("00P00001Oa@00002OaB`03l4Ob9`Ogl8@28007l@@000040P@000001000000000")
          .Append("O`00@GP0Oa@8@Ga13Q@8=PA171@827ao3Q@800AoOa@807`0O`0007P000000000")
          .Append("O`000W`4Ob800g`2OR9P0GP171AP0``2>1@00P@4O`P00g`0O`000GP000000000")
          .Append("?PI0L3R0O`LPN7b0Oe<@C7b0@ET8ATB0@El4C4B0O`l2N7b0?PH1L3R000000020")
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
          ' ToDo: Add font sheet from Contra / Time Pilot '84
          .Append("")
        Case BuiltinFont.RollThunder
          ' ToDo: Add font sheet from Rolling Thunder 2
          .Append("")
        Case BuiltinFont.Xevious
          ' ToDo: Add font sheet from Xevious (by Namco)
          .Append("")
        Case BuiltinFont.Guardian
          ' ToDo: Add font sheet from Guardian (by Taito)
          .Append("")
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
