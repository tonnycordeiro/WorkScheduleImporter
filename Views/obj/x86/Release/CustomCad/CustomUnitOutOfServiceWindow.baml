   M S B A M L   `   `   `  ���� !  Sisgraph.Ips.Samu.AddIn.ViewsJ    DSisgraph.Ips.Samu.AddIn.Views.CustomCad.CustomUnitOutOfServiceWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  Fora de Serviço��$	��280��$	��600�� � 
ResizeMode$ NoResize=� � 	Resources  �� Source�� $ ../DefaultResources.xaml?�%    .��]� .+��� .+�]� $��5q�.+��� $	��100��$��Right=�.��
Viatura:�� -    ��Name$ 	txtUnitId��$��80�� �� 
IsReadOnly . 
�������$��5,0,0,0q����� *UnitId+�� $��Right=�.��Local:�� -   $ txtLocation��$	��230��$��5,0,0,0q����� *
Location+�� $��Right=�.��Km:�� -   $ 
txtMileage��$��5,0,0,0q����� *	Mileage+]� $��5,0,5,5q�.+��� $	��100��$��Right=�.��Código de Saída:�� -    ��Name$ 
cmbOutType��$	��315��$��5,0,0,0q� �� SelectedValuePath$ OutServiceTypeId�� �� DisplayMemberPath$ OutServiceTypeDescription������ *OutTypeList+	 �� SelectedItem	 �� *SelectedOutType+�� $	��100��$��Right=�.��Tempo de Alarme:�� -   $ txtAlarmTime��$��5,0,0,0q����� *	AlarmTime+�� $��5,0,5,0q�$
��Left=�.��Comentários:�� -   $ 
txtRemarks��$	��100��
 �� TextWrapping$

 Wrap=� �� AcceptsReturn . $��5,0,5,5q����� *	Remarks+]� .+��� -    ��Name$ btnExit��$��5,0,0,0q�$
��Left=�$
��Left=�   DefaultTextImageButton#��[   � ToolTip$ Retornar��.��]� $	��138��$��42��$��5q�.+��� $>��8/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exit.png��$��Right=��� $��Center=�$
��Left=�
Retornar�� -   $ 
btnConfirm��$��0,0,5,0q�$��Right=�$
��Left=�#��[  $ 	Confirmar��.��]� $	��138��$��42��$��5q�.+��� $?��9/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Check.png��$��Right=��� $��Center=�$
��Left=�	Confirmar]� 