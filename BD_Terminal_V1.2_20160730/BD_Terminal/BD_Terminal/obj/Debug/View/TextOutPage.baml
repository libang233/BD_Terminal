   M S B A M L   `   `   `  ����F  BBD_Terminal, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:BD_Terminal.ViewBD_Terminal.View  5      "    BD_Terminal.View.TextOutPage   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:BD_Terminal.View     5        N� 
Background
  ����� N� Title$ TextOutPage��5	      .��5      � .+�5   
   �� .+�5      � .+�5      �� $��
User Debug��6   $��15(�6-   ���	#FFADBCC96;   5      � 5      .+�5      �� $��0,0,0,0q�6   .+�5         BD_Terminal.View.Mbutton  -   5          Name$ 	btn_clear��5   $   $��50��67   $��50��6B      mContent$ CLEAR��6N      MState .  $��Center=�5      $��0,0,0,0q�66      
ClickEvent Btn_ClearOperation6G     5      -   5        $ btn_stopRoll��5   $   $��50��6:   $��50��6E   $
 STOP��6Q    .  $��Center=�5      $��0,5,0,0q�66    Btn_StopRollOperation6G     5      -   5        $ ControlSaveFile��5   $   $��50��6=   $��50��6H   $
 SAVE��6T    .   Btn_Save_To_File6s   $��0,5,0,0q�6�   5      �� 5      -   5         ��Name$ textBox��5      $�� ��6+   $��Center=�63   $��5,5,5,5q�6P   ���	#FFADBCC9$��2q�5   /   $	��440��6C    �� TextWrapping$
 Wrap=�5      $	��Top=�6+   
�������$
��Auto=�5      $	��510��6F   $��10,0,0,0q�6R   $ �� HorizontalScrollBarVisibility$
 Auto=�6d   ��. 	 �� 	UndoLimit$	 0��5   >   
 �� IsUndoEnabled
 .   �� 
IsReadOnly . 6b   5      5       5!      5"      