   M S B A M L   `   `   `  ���� G  CAppStandards, Version=2.1.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:AppStandards.UIControlsAppStandards.UIControls  *    $AppStandards.UIControls.SplashScreen   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     7local%clr-namespace:AppStandards.UIControls     Ishell9http://schemas.microsoft.com/netfx/2009/xaml/presentation      � 
ResizeMode$  NoResize=�$��413.547��$��222.66��   WindowStyle#���    � 	Resources  �� 
TargetType �� Property �� Value'  !System.Windows.Shell.WindowChrome   ResizeBorderThickness   CornerRadius   GlassFrameThickness   CaptionHeight	 �� 
TargetType
 �� Padding K� ZIndex �� RelativeSource �� Path � RowDefinitions �Name+  %AppStandards.UIControls.CornerButtons   	CanCancel��%0  &	        �� " �.���  � ��$ 50����  � ��$ 50����  �@ WindowChrome   $ 5q�$ 0��$ 0q�$ 30Q���  � �� �� "	 �.R��� $��0q�$
 0q�$ -1��$��2q����	#FF2D2D30����  �� *TemplatedParent+$ 
Background�._��  �� $��33 ��� .+�� $ titlebar��
������ .+��� $��5q�$��Center=�$
��Left=�
������ 
�������$��14(�����  �� *TemplatedParent+$ Title�  $��Right=� ��  �� *TemplatedParent+$ 	CanCancel��� $��1��.u��� #��z��.��]� .+�]� $��Bottom=���.  .+��� $
��Left=� �� Orientation$ 
Horizontal=�.+��� $��Produced by: ��$��5,0,0,5q��� $��0,0,0,0q����� *AppInfo.Company+�� $��Right=�$ 
Horizontal=�.+��� $��	Version: ��$��0,0,0,5q��� $��0,0,5,5q����� *AppInfo.VersionNumber+�� $	��Top=�$��Title��$��Center=�$��Center=�