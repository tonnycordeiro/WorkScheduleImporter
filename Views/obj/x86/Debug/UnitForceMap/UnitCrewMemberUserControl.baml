   M S B A M L   `   `   `  ����Y  USisgraph.Ips.Samu.AddIn.Models, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null�aclr-namespace:Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;assembly=Sisgraph.Ips.Samu.AddIn.Models+Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap  5      ! Sisgraph.Ips.Samu.AddIn.ViewsJ   DSisgraph.Ips.Samu.AddIn.Views.UnitForceMap.UnitCrewMemberUserControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      tmodelsaclr-namespace:Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;assembly=Sisgraph.Ips.Samu.AddIn.Models     5      .��5      �� .+�5   
   �� ��6   �� *Label+��5      �� 5        �� 
TargetType"  ��6   .�5      ��  �� Property � �� �� Value$ 0,5,0,0q�6/   5      5      � 5   
    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��5 �6#   �� 5      $
��Auto �6#   .+�5      �� -   5         �� CharacterCasing$ Upper=�5   �   ��6   �� *Value+ �� UpdateSourceTrigger$ PropertyChanged=�.��6�   ��5      �� 5      "  ��6    �� BasedOn# [B��61   .�5      ��  � ��$ 0q�63   6?    �� Triggers 5       �� 5!      I   CSisgraph.Ips.Samu.AddIn.Models.UnitForceMap.UnitCrewMemberStateEnum   	Verifying	 �� Value#	 Z 6E   
 �� Binding
 6*   �� *Result+.a�5"   "   ��  �@��
IsReadOnly . 6?   �� 5#   "    � ��
 �����6?   �� 5$   "    �@�Cursor$
 Wait��6;   5%      5'      .��5(   !   5)      �� 5+      -   5        $��2��5+      ��5,      �� 5-      "  ��6   .�5.      ��  � ��$ 20��62   �� 5/       � ��
 �����67   �� 50       � |� .  66   51      ��53      �� 54      .R�55      �� ��56      �� 57   "   "  ��6(   .�58   &   ��  � ��$@ :/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Delete.png��6?   6�    59   &   �� 5:   *   	   Ok#	 Z 6Q   
 66   �� *Result+.a�5;   .   ��  � ��$? 9/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Check.png��6G   6�   5<   +   �� 5=   *      Multiple#	 Z 6Q   
 66   �� *Result+.a�5>   .   ��  � ��$A ;/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Warning.png��6G   6�   5?   +   �� 5@   *   #	 Z 6Q   
 66   �� *Result+.a�5A   .   ��  � ��$A ;/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Refresh.png��6G   6�   5B   +   5D   #   5F      5G      5I      5K      �� 5M   
   ��6   �� *	Message+��5N      �� 5O      "  ��6   .�5P      ��  � ��$
 Bold"�63   6B   �� 5Q       � ��
 �  ��6A   �� 5R       � ��$ 0q�6/   6;    5S      �� 5T      #	 Z 6A   
 6&   �� *Result+.a�5U      ��  � ��
 ��  �6J   5V      �� 5W      #	 Z 6A   
 6&   �� *Result+.a�5X      ��  � ��
 � � �6K   5Y      5[      5]      5_      5a      