   M S B A M L   `   `   `  ����!  Sisgraph.Ips.Samu.AddIn.ViewsK    ESisgraph.Ips.Samu.AddIn.Views.CustomCancelEvent.CustomCancelEventMain   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Cancelar Ocorrência��5   	   $	��310��6%   $	��400��62    � WindowStartupLocation$ CenterScreen=�6>    � 
ResizeMode$ NoResize=�6c    � 	Resources 5       �� Source�� 5   
   $ ../DefaultResources.xaml?�6   %    6A   .��5	      �� $��5q�6   .+�5
   
   �� $��Motivo do Cancelamento��6   �� 5   
    �� DisplayMemberPath$ Value��6j    �� SelectedValuePath$	 Key��6�   ��6   �� *CancelReasonList+ �� SelectedItem 6<   �� *SelectedCancelReason+6�   �� 5   
   $��Observação��6   �� 5   
    �� TextWrapping$
 Wrap=�6.   	 �� AcceptsReturn	 . $	��150��6W   $��0,0,0,5q�6d   ��6   �� *
RemarkText+6d   ]� 5   
   $��Right=�6   .+�5      �� -   5        
 ��Name$
 
btnConfirm��5   ,   $��Right=�6   $��Right=�6@      DefaultTextImageButton#��[  6u    � ToolTip$ 	Confirmar��6�   .��5      ]� $	��138��6   $��42��6(   $��5q�64   .+�5      �� $?��9/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Check.png��6   $��Right=�6_   6x   �� 5      $��Center=�6    	Confirmar6F   5      5      �� 5      -   5        $
 btnBack��5   +   $
��Left=�6   $��Right=�6<   #��[  6n   $ Voltar��6�   .��5      ]� $	��138��6   $��42��6(   $��5q�64   .+�5      �� $>��8/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exit.png��6   $��Right=�6^   6w   �� 5      $��Center=�6    Voltar6C   5      5      ]� 5      5      5      5      