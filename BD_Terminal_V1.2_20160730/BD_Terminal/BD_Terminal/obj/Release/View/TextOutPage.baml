   M S B A M L   `   `   `  ���� F  BBD_Terminal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BD_Terminal.ViewBD_Terminal.View  "    BD_Terminal.View.TextOutPage   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0localclr-namespace:BD_Terminal.View       N� 
Background
  ����� N� Title$ TextOutPage��.��� .+��� .+�� .+��� $��
User Debug��$��15(����	#FFADBCC9� .+��� $��0,0,0,0q�.+�   BD_Terminal.View.Mbutton  -     Name$ 	btn_clear��$��50��$��50��   mContent$ CLEAR��   MState .  $��Center=�$��0,0,0,0q�   
ClickEvent Btn_ClearOperation  -   $ btn_stopRoll��$��50��$��50��$
 STOP�� .  $��Center=�$��0,5,0,0q� Btn_StopRollOperation  -   $ ControlSaveFile��$��50��$��50��$
 SAVE�� .   Btn_Save_To_File$��0,5,0,0q��� -    ��Name$ textBox��$�� ��$��Center=�$��5,5,5,5q����	#FFADBCC9$��2q�$	��159�� �� TextWrapping$
 Wrap=�$	��Top=�
�������$
��Auto=�$	��511��$��10,0,0,0q�$ �� HorizontalScrollBarVisibility$
 Auto=���. 	 �� 	UndoLimit$	 0��
 �� IsUndoEnabled
 .   �� 
IsReadOnly . 