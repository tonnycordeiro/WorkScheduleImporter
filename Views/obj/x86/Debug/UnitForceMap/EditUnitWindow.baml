   M S B A M L   `   `   `  ����X  TSisgraph.Ips.Samu.AddIn.Views, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullg8clr-namespace:Sisgraph.Ips.Samu.AddIn.Views.UnitForceMap*Sisgraph.Ips.Samu.AddIn.Views.UnitForceMap  5   	   ?    9Sisgraph.Ips.Samu.AddIn.Views.UnitForceMap.EditUnitWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jviews8clr-namespace:Sisgraph.Ips.Samu.AddIn.Views.UnitForceMap     5   	     � WindowStyle$  SingleBorderWindow=�5   	    � WindowStartupLocation$ CenterScreen=�6*    � SizeToContent$ WidthAndHeight=�6O    � 
ResizeMode$ NoResize=�5   	   
������� � Title$ Edição de Dados��67    � 	Resources 5       �� Source�� 5   
   $ ../DefaultResources.xaml?�6   %    6A   .��5      ]� .+�5   
   �� $	��Top=�6   $��Center=�6H   ��6)   �� *HeaderText[0]+6H   �� 5   
   $	��Top=�6   $��24(�6H   $
��Bold"�6V   $��Center=�6h   ��6)   �� *HeaderText[1]+6h   �� 5   
   $	��Top=�6   $��0,0,0,5q�6H   $��Center=�6Y   ��6)   �� *HeaderText[2]+6Y   �� 5   
   $	��Top=�6   
������� 5       �� 
TargetType �� Property	 �� Value
 �� BasedOn��%�   '��       '�� I     7���� 5      " ��6   .�5      ��  � ��$		 400��6.   5      �� 5      " ��6   8
   .�5      ��  � ��$	 0,5,0,0q�6/   �� 5       � ��$	 0q�60   5      5      .@�5      �� -   5         ��Name$ 
tabGeneral��5      $��Geral��6*   
�������.��5      �� .+�5      �� .��6   	Unidade6%   �� 5         TextBoxDisabled#��[  6C   ��6   �� *SelectedUnit.UnitId+6C   �� 5       .��6   	Celular6%   �� 5!       �� 	MaxLength$ 12��6F   ��6   �� *SelectedUnit.CellPhone+6F   �� 5#      .��6   Grupo6#   �� 5$      -   5        ��5$      �� *	GroupList+ �� SelectedItem 6A   �� *SelectedDispatchGroup+6p   �� 5&      .��6   Base6"   �� 5'       �� DisplayMemberPath$ Station��6C    �� SelectedValuePath$ 	StationId��6_   ��6   �� *StationList+ 5(      �� *SelectedUnit.Station+�� 5*      .��6   Observações6)   �� 5+       �� TextWrapping$
 Wrap=�6    �� AcceptsReturn .  �� 
AcceptsTab . $��90��6Y   $
��Auto=�6e   ��6�   �� *SelectedUnit.Remarks+6�   5,      5-      �� 5/      -   5        $ tabTeam��5/      $��Equipe��6'   
�������.��50      �� .+�52      �� $��	Categoria��6   $��5q�62   .��53      �� $	��380��6%   .+�54      	� $��0,0,0,5q�6G    d� 	IsChecked 6*   �� *IsURAM+.��6X   URAM6^   	� 55      $��Center=�6w    6*   �� *IsURAM+  NotBooleanConverter �� 	Converter# [ ��56   "   �� 57   &    �� Orientation$ 
Horizontal=�61   $	��250��6J   .+�58   *   �� $��0q�64   $��Center=�6?   Outra:6b   �� 59   *   .��62   ��5:   .   �� 5;   2   " ��68   #
 [  6M   .�5<   6   ��  � ��$		 200��6N   6\   �� 5=   6    � ��$	 5,0,0,0q�6O   6a   �� 5>   6    � ��	 6M   �� *SelectedUnit.Category+6w    �� Triggers 5?   6   �� 5@   :    �� Binding 6F   �� *IsURAM+ �� Value True6a   .a�5A   >   ��  � {�	 .  6j   �� 5B   >    � ��$	 -��6U   6a   5C   ;   5E   3   .��5F   =   5G   +   5H   '   5J      5K      5L      J   DSisgraph.Ips.Samu.AddIn.Views.UnitForceMap.UnitCrewMemberUserControl  5N       � DataContext 66   �� *
DriverVM+�� 5O      {�6!   �� *IsURAM+# [ .+�5P         6:   �� *
DoctorVM+  5Q       6:   �� *	NurseVM+  5R       6:   �� *FirstAuxiliarVM+  5S       6:   �� *SecondAuxiliarVM+5U      5V      5W      5X      ]� 5Z   
   $��0,5,0,0q�6   .+�5[      �� -   5         ��Name$ 	btnCancel��5[        DefaultTextImageButton#��[ 6=    � ToolTip$ Cancelar��6m   .��5\      � $	��138��6   $��42��6#    � ColumnDefinitions 5]      �� 5^      $��* �6+   67   �� 5_      $
��Auto �6+   6:   .+�5b      �� $��Center=�6    
Cancelar6E   �� 5c      $>��8/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exit.png��6   $��1��6^   6p   5d      5e      �� 5f      -   5        $ 
btnConfirm��5f      $��Right=�6)   #��[ 6[   $ 	Confirmar��6�   .��5g      � $	��138��6   $��42��6#    5h      �� 5i      $��* �6+   67   �� 5j      $
��Auto �6+   6:   .+�5m      �� $��Center=�6    	Confirmar6F   �� 5n      $>��8/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Save.png��6   $��1��6^   6p   5o      5u      5v      5w      5x      