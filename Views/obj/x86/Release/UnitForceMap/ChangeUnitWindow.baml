   M S B A M L   `   `   `  ���� !  Sisgraph.Ips.Samu.AddIn.ViewsA    ;Sisgraph.Ips.Samu.AddIn.Views.UnitForceMap.ChangeUnitWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     
�������  � Title$  Substituir AM�� � SizeToContent$ Height=�$	��520�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=� � 	Resources  �� Source�� $ ../DefaultResources.xaml?�%    .���� $��5,0,5,0q�.+��� $��Substituição de AM��$��Center=�$
��Bold"�]� .+��� $��Substituição da AM: ��$
��Left=�$	��140����  �� 
IsReadOnly . $��5,0,0,0q�����  �� Path$  CurrentUnitForceMap.UnitId�]� $��0,5,0,0q�.+��� $ ��Motivo da Substituição: ��$
��Left=�$	��140����  �� DisplayMemberPath$ OutServiceTypeDescription��	 �� SelectedValuePath$	 OutServiceTypeId��$��5,0,0,0q����� *OutServiceTypeList+
 �� SelectedItem
 �� *SelectedChangeReason+�� $��Nova AM��$��0,5,0,5q�.���� .+�]� $��0,5,0,0q�.+��� $��	Viatura: ��$
��Left=�$	��130��$��Right=��� $��5,0,0,0q����� *ReserveUnitList+
 �� *SelectedTargetUnitId+]� .+��� -    ��Name$ btnBack��$
��Left=�   DefaultTextImageButton#��[   � ToolTip$ Voltar��.��]� $	��138��$��42��$��5q�.+��� $>��8/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exit.png��$��Right=��� $��Center=�$
��Left=�Voltar�� -   $ btnSave��$��Right=�#��[  $ 
Substituir��.��]� $	��138��$��42��$��5q�.+��� $B��</Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exchange.png��$��Right=��� $��Center=�$
��Left=�
Substituir