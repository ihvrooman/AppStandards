   M S B A M L   `   `   `  ����H  DAppStandards, Version=2.0.11.3, Culture=neutral, PublicKeyToken=nullA%clr-namespace:AppStandards.UIControlsAppStandards.UIControls  5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089   WindowStyle  �� 
TargetType �� Property �� Value'   !System.Windows.Shell.WindowChrome    ResizeBorderThickness    CornerRadius    GlassFrameThickness    CaptionHeight �� 
TargetType �� Padding	 K� ZIndex
 � RowDefinitions �Name �� RelativeSource �� Path+  %AppStandards.UIControls.CornerButtons   	CanCancel  ButtonStyle �� RadiusX �� RadiusY  RoundCorner ��Name �� CornerRadius 
� GradientOrigin �� RelativeTransform �� CenterX �� CenterY �� ScaleX �� ScaleY T� X T� Y � Offset � Color �� Triggers P� Property P� Value  �� 
TargetName�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    Ishell9http://schemas.microsoft.com/netfx/2009/xaml/presentation    5      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      6userControls%clr-namespace:AppStandards.UIControls  5      %�	  &	        &	 r    &	 G    �� 5      "  �6    .�5   
   ��  � ��$ 50��6&   �� 5	   
    � ��$ 50��6%   �� 5
   
    �@  WindowChrome 5         5      $ 5q�6%   $ 20��6?   $ 0q�6Q   $ 30Q�6i   5      �� 5   
    � �� 5      �� 5      " �6"   .R�5      �� $��0q�6   $ 0q�6(   
�������$	 -1��6G   $��2q�6X   ���	#FF2D2D30._�5      � 
 5      �� 5   "   $��33 �60   �� 5   "   .+�5      � $ titlebar��66   
�������.+�5   "   �� $��5q�6y   $��Center=�5   !   $
��Left=�6<   
������ 
�������$��14(�6�   ��5   ,   ��  �� *TemplatedParent+$ Title�5   �     5   "   $��Right=�6=    6Y   ��  �� *TemplatedParent+$ 	CanCancel�5       �� 5#      $��1��6/   .u�5$   "   �� #��z��63   5%      5'      5(      5*      5,      5-      �� 5/      "  ��6    �50   
   �� 51       � �� 52      �� 53      " ��6&   .R�54      �� $	��100��6$   $��40��60   $ 15��6<   $ 15��6I   
�������6V   55      57      59      �� 5;      "  ��6    .�5<   
   ��  � ��$ Center=�67   �� 5=   
    � ��$ Center=�65   �� 5>   
    � ��$ 1q�6$   �� 5?   
    � �� 5@      �� 5A      " ��6"   .R�5B      � $
 grid��6   .+�5C      �� $ border��6!   $ 8��61   
���   �$��2q�6V   ��5D      
� 5E   "   $ 0.496,1.052&�66    5F   &   V� 5G   *   .~�5H   .   �� $	 0.5Q�6=   $	 0.5Q�6K   $	 1.5Q�5I   9   $	 1.5Q�6F   T� 5J   .   $
 0.02Q�6A   $	 0.3Q�6J   5K   +   .��5M   &   � $ 1Q�63   $ 	#00000000��6>   � 5N   &   $	 0.3Q�63   $ 	#FFFFFFFF��6@   5O   #   ._�5Q      �� $��Center=�6/   $��Center=�5R   +   $
��Bold"�5S   +   5T      5U      5W       5X      P� 5Y       � �� . . �5Z      �� $  border��6;    � �� 5[   "   
� 5\   &   $ 0.496,1.052&�6:    5]   *   V� 5^   .   .~�5_   2   �� $	 0.5Q�6A   $	 0.5Q�6O   $	 1.5Q�6]   $	 1.5Q�6j   T� 5`   2   $
 0.02Q�6E   $	 0.3Q�6N   5a   /   .��5c   *   � $ 	#00000000��67   $ 1Q�6I   � 5d   *   $ 	#FF303030��67   $	 0.3Q�6I   5e   '   5g      5h      P� 5i       �@J�IsMouseOver . . �5j      �� $  border��6<    � �� �	#FF33962B6P   5k      P� 5l       � {� .  . �5m      �� $
  grid��68    �@J�Opacity$
 0.25Q�6J   5n      5q      5s      5t      5v      