   M S B A M L   `   `   `  ����!  Sisgraph.Ips.Samu.AddIn.ViewsA    ;Sisgraph.Ips.Samu.AddIn.Views.UnitForceMap.ChangeUnitWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   
�������  � Title$  Substituir AM��5   	    � SizeToContent$ Height=�6   $	��520��66    � 
ResizeMode$ NoResize=�6B    � WindowStartupLocation$ CenterScreen=�6X    � 	Resources 5       �� Source�� 5   
   $ ../DefaultResources.xaml?�6   %    6A   .��5      �� $��5,0,5,0q�6   .+�5	   
   �� $��Substituição de AM��6   $��Center=�6-   $
��Bold"�6J   6^   ]� 5
   
   .+�5      �� $��Substituição da AM: ��6   $
��Left=�63   $	��140��6I   6W   �� 5       �� 
IsReadOnly . $��5,0,0,0q�6Y   ��6(   ��  �� Path$  CurrentUnitForceMap.UnitId�6l   5      ]� 5   
   $��0,5,0,0q�6   .+�5      �� $ ��Motivo da Substituição: ��6   $
��Left=�67   $	��140��6M   6[   �� 5       �� DisplayMemberPath$ OutServiceTypeDescription��6   	 �� SelectedValuePath$	 OutServiceTypeId��6E   $��5,0,0,0q�6�   ��6j   �� *OutServiceTypeList+
 �� SelectedItem
 6�   �� *SelectedChangeReason+6�   5      �� 5   
   $��Nova AM��6   $��0,5,0,5q�6$   .��5      �� .+�5      ]� $��0,5,0,0q�6   .+�5      �� $��	Viatura: ��6   $
��Left=�60   $	��130��6F   $��Right=�6R   6p   �� 5      $��5,0,0,0q�6u   ��6   �� *ReserveUnitList+
 6G   �� *SelectedTargetUnitId+5      5      5      5      ]� 5   
   .+�5      �� -   5         ��Name$ btnBack��5      $
��Left=�6&      DefaultTextImageButton#��[  6W    � ToolTip$ Voltar��6�   .��5      ]� $	��138��6   $��42��6(   $��5q�64   .+�5      �� $>��8/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exit.png��6   $��Right=�6^   6w   �� 5      $��Center=�6    $
��Left=�6;   Voltar6Y   5       5!      �� 5#      -   5        $ btnSave��5#      $��Right=�6&   #��[  6X   $ 
Substituir��6�   .��5$      ]� $	��138��6   $��42��6(   $��5q�64   .+�5%      �� $B��</Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exchange.png��6   $��Right=�6b   6{   �� 5&      $��Center=�6    $
��Left=�6;   
Substituir6]   5'      5(      5)      5*      5+      