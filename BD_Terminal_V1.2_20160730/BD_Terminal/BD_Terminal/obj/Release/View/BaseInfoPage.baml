   M S B A M L   `   `   `  ���� F  BBD_Terminal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BD_Terminal.ViewBD_Terminal.View  #    BD_Terminal.View.BaseInfoPage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:BD_Terminal.View       N� 
Background
  ����� N� Title$ BaseInforPage��$	��570��.��� .+��� $
��Left=�$	��570��$��0,0,0,0q�$	��Top=�$	��200��.+�� $	��120��.+��� $��	0,0,0,-17q�.+��� $��Config��$
��Left=�$��15(����	#FFADBCC9$��0,0,0,0q�� $��Center=�$��24��$��0,0,0,0q�$	��Top=�$	��184��.+��� -    ��Name$ lab_com��$	��Com��$��66��$��24���� -    ��Name$ commbox_com��$	��111��$��20��� $��Center=�$��27��$	��8,0q�$	��Top=�$	��184��.+��� -   $ lab_baudRate��$��Baudrate��$��66��$��27���� -   $ combox_baudRate��$	��111��$��13.33(�$��Center=�$��20������  �� EndPoint$	 0,1&� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFF0F0F0�� � Offset$ 0Q�� $ White��$ 1Q��� -    ��Name$ btn_connect_com��$��Center=�$��0,10,0,0q�$	��Top=�$	��180��
�������$��22������ 	 �� 
TargetType"	 ��.R��� $��PART_Background�����	#FF457CDC$	��1.5q�
 �� CornerRadius$
 8,8,8,8��._��� #��z��$��Center=�$��Center=��� $��2�����	#FFADBCC9$	��120��$��0,13,0,0q�$
��Left=��� -   
������ 
������ $��
10,90,10,8q��� -   $ label��$��	Base Info��$
��Left=�$��15(����	#FFADBCC9$��0,5,0,0q�� $��25��$��0,5,0,0q�.+��� $��经度��$��80��$��Center=��� -   $ label_longitude��$�� ��$	��120��$��25��� $��25��.+��� $��纬度��$��80��$��Center=��� -	   $ lable_latitude��$�� ��$	��120��$��25��� $��25��.+��� $��	UTC时间��$��80��$��Center=��� -
   $ lable_UTCtime��$	��120��$��25��� $��25��.+��� $��海拔高度��$��80��$��Center=��� -   $ lable_altitude��$�� ��$	��120��$��25��� $��25��.+��� $��椭球高度��$��80��$��Center=��� -   $ lable_Ellipsoidal_height��$	��120��$��25��� $��27��.+��� $��	卫星数��$��80��$��Center=��� -   $ lable_satellite��$�� ��$	��120��$��28��� $��25��.+��� $��定位精度��$��80��$��Center=��� -   $ lable_pos_acc��$�� ��$	��120��$��25��� $��25��.+��� $��伪距��$��80��$��Center=��� -   $ lable_pseudorange��$
��----��$	��120��$��25��� $��25��.+��� $��	伪距率��$��80��$��Center=��� -   $ lable_pseudorangeRate��$
��----��$	��120��$��25��� $��25��.+��� $
��GDOP��$��80��$��Center=��� -   $ 
lable_GDOP��$
��----��$	��120��$��25��� $��25��.+��� $
��PDOP��$��80��$��Center=��� -   $ 
lable_PDOP��$�� ��$	��120��$��25��� $��27��.+��� $��高斯坐标 :X��$��80��$��Center=��� -   $ lable_GAS_coordination_X��$�� ��$	��120��$��28��� $��27��.+��� $��高斯坐标 :Y��$��80��$��Center=��� -   $ lable_GAS_coordination_Y��$�� ��$	��120��$��28���� $��2�����	#FFADBCC9$	��120��$��0,5,0,0q�$
��Left=�� $��80��.+��� $��定位状态��$��80��$��Center=�$��0,20,0,0q�   BD_Terminal.View.GpsState  -     Name$ GpsState��$��40��$��40��$��0,15,0,0q�� -    �Name$ LongStd_Grid��$��80,171,19,372q�
�������$y�Hidden=�.+��� ���	#FFADBCC9$��1��� .+��� $
��STD:���� -   $ label_longstd��$��64��� -   $ LatStd_Grid��$��80,198,19,345q�
�������$y�Hidden=�.+��� ���	#FFADBCC9$��1��� .+��� $
��STD:��$��36���� -   $ label_latstd��$��64��$��27���� -   $��0,158,0,385q����	#00000000�� -   $��0,185,0,361q����	#00000000