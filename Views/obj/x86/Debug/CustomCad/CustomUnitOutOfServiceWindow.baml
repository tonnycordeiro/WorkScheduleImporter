   M S B A M L   `   `   `  ����!  Sisgraph.Ips.Samu.AddIn.ViewsJ    DSisgraph.Ips.Samu.AddIn.Views.CustomCad.CustomUnitOutOfServiceWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Fora de Serviço��5   	   $	��280��6!   $	��600��6.    � 
ResizeMode$ NoResize=�6:    � 	Resources 5       �� Source�� 5   
   $ ../DefaultResources.xaml?�6   %    6A   .��5      ]� .+�5	   
   �� .+�5
      ]� $��5q�6   .+�5      �� $	��100��6   $��Right=�6$   .��6G   
Viatura:6Q   �� 5      -   5         ��Name$ 	txtUnitId��5      $��80��6-    �� 
IsReadOnly . 
�������$��5,0,0,0q�6b   ��6s   �� *UnitId+6�   �� 5      $��Right=�6   .��6;   Local:6C   �� 5      -   5        $ txtLocation��5      $	��230��6/   $��5,0,0,0q�6;   ��6L   �� *
Location+6h   �� 5      $��Right=�6   .��6;   Km:6@   �� 5      -   5        $ 
txtMileage��5      $��5,0,0,0q�6.   ��6?   �� *	Mileage+6Z   5      ]� 5      $��5,0,5,5q�6   .+�5      �� $	��100��6   $��Right=�6$   .��6G   Código de Saída:6Y   �� 5      -   5         ��Name$ 
cmbOutType��5      $	��315��6/   $��5,0,0,0q�6;    �� SelectedValuePath$ OutServiceTypeId��6p    �� DisplayMemberPath$ OutServiceTypeDescription��6�   ��6L   �� *OutTypeList+	 �� SelectedItem	 5      �� *SelectedOutType+6u   �� 5      $	��100��6   $��Right=�6$   .��6G   Tempo de Alarme:6Y   �� 5      -   5        $ txtAlarmTime��5      $��5,0,0,0q�60   ��6A   �� *	AlarmTime+6^   5      �� 5      $��5,0,5,0q�6   $
��Left=�6%   .��6G   Comentários:6U   �� 5      -   5        $ 
txtRemarks��5      $	��100��6*   
 �� TextWrapping$

 Wrap=�67    �� AcceptsReturn . $��5,0,5,5q�6`   ��6q   �� *	Remarks+6�   ]� 5       .+�5!      �� -   5         ��Name$ btnExit��5!      $��5,0,0,0q�6@   $
��Left=�6Q   $
��Left=�6g      DefaultTextImageButton#��[  6�    � ToolTip$ Retornar��6�   .��5"      ]� $	��138��6    $��42��6,   $��5q�68   .+�5#      �� $>��8/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exit.png��6    $��Right=�6b   6{   �� 5$      $��Center=�6$   $
��Left=�6?   
Retornar6_   5%      5&      �� 5'      -   5        $ 
btnConfirm��5'      $��0,0,5,0q�6F   $��Right=�6W   $
��Left=�6n   #��[  6�   $ 	Confirmar��6�   .��5(      ]� $	��138��6    $��42��6,   $��5q�68   .+�5)      �� $?��9/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Check.png��6    $��Right=�6c   6|   �� 5*      $��Center=�6$   $
��Left=�6?   	Confirmar6`   5+      5,      ]� 5-      5.      5/      50      51      