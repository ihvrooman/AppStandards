   M S B A M L   `   `   `  ����H  DAppStandards, Version=2.0.11.3, Culture=neutral, PublicKeyToken=nullA%clr-namespace:AppStandards.UIControlsAppStandards.UIControls  5      *    $AppStandards.UIControls.SplashScreen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7local%clr-namespace:AppStandards.UIControls     5      Ishell9http://schemas.microsoft.com/netfx/2009/xaml/presentation    5        � 
ResizeMode$  NoResize=�5	      $��413.547��5
      $��222.66��5         WindowStyle#���   5       � 	Resources 5       �� 
TargetType �� Property �� Value'  !System.Windows.Shell.WindowChrome   ResizeBorderThickness   CornerRadius   GlassFrameThickness   CaptionHeight	 �� 
TargetType
 �� Padding K� ZIndex �� RelativeSource �� Path � RowDefinitions �Name+  %AppStandards.UIControls.CornerButtons   	CanCancel��%�  &	        �� 5   
   " �6$   .�5      ��  � ��$ 50��6*   �� 5       � ��$ 50��6)   �� 5       �@ WindowChrome 5        5      $ 5q�6)   $ 0��6C   $ 0q�6T   $ 30Q�6l   5      �� 5       � �� 5      �� 5      "	 �6&   .R�5      �� $��0q�6!   $
 0q�6,   $ -1��6�   $��2q�6�   ���	#FF2D2D30��68   ��  �� *TemplatedParent+$ 
Background�._�5      �  5   "   �� 5   &   $��33 �64   �� 5   &   .+�5#   "   � $ titlebar��6@   
������ .+�5$   &   �� $��5q�6}   $��Center=�5%   !   $
��Left=�6<   
������ 
�������$��14(�6�   ��5$   0   ��  �� *TemplatedParent+$ Title�5%   �     5&   &   $��Right=�6:    6V   ��  �� *TemplatedParent+$ 	CanCancel�5'   #   �� 5*   "   $��1��63   .u�5+   &   �� #��z��67   5,   #   5.      5/      51      53      54      55      .��57      ]� .+�58   
   ]� $��Bottom=�6   ��.  .+�59      �� $
��Left=�6    �� Orientation$ 
Horizontal=�6/   .+�5:      �� $��Produced by: ��6   $��5,0,0,5q�61   �� 5;      $��0,0,0,0q�6=   ��6   �� *AppInfo.Company+6=   5<      �� 5=      $��Right=�6   $ 
Horizontal=�60   .+�5>      �� $��	Version: ��6   $��0,0,0,5q�6-   �� 5?      $��0,0,5,5q�6C   ��6   �� *AppInfo.VersionNumber+6C   5@      5A      �� 5B   
   $	��Top=�6   $��Title��6)   $��Center=�66   $��Center=�6S   5C      5E      