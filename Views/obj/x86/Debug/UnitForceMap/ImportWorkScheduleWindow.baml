   M S B A M L   `   `   `  ����!  Sisgraph.Ips.Samu.AddIn.ViewsI    CSisgraph.Ips.Samu.AddIn.Views.UnitForceMap.ImportWorkScheduleWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     6@   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6}   
�������  � Title$  Importar Escala��5   	    � SizeToContent$ Height=�6!   $��1000.377��68    � 
ResizeMode$ NoResize=�6I    � WindowStartupLocation$ CenterScreen=�6_   $	��600��6�    � 	Resources 5       �� Source�� 5   
   $ ../DefaultResources.xaml?�6   %    .��5	      �� $��5,0,5,10q�6   .+�5
   
   �� $��Importação de Escala��6   $��Center=�6/   $
��Bold"�6L   $��18(�6^   �� 5   
   -   5        $��5,0,5,10q�5      $��15(�6%    d� 	IsChecked 6m   �� *IsPeriodicWorkSchedule+.��6�   Criar escala periódica6�   �� 5   
   -   5        $��GroupPeriodicWorkSchedule��5   /   $��Escalas Periódicas��6   $��5,0,5,10q�6P   .��5      �� .+�5      �� $��5,0,5,10q�6    �� Orientation$ 
Horizontal=�6/   .+�5      �� $��Data inicial: ��6   $
��Left=�65   $��90��6K   $��32��6V   $��15(�6b   (  "System.Windows.Controls.DatePicker  5      -   5        $��StartDatePicker��5   !   $	��160��6[   $��25��6g      SelectedDate 68   �� *	StartDate+6g   �� 5      $��Data final: ��6   $
��Left=�63   $��90��6I   $��32��6T   $��30 0 0 0q�6`   $��15(�6r     5      -   5        $��EndDatePicker��5   !   $	��160��6W   $��25��6c    66   �� *	EndDate+6c   5      �� 5      $ 
Horizontal=�6   $��5,0,5,10q�66   .+�5      �� $��Tipo de importação: ��6   $
��Left=�6;   $	��150��6Q   $��32��6]   $��15(�6i   	� 5      -   5        $!��RadioBtCreatingWorkSchedule��5   "   $��Center=�6E   	 	� 	GroupName$	 PeriodicWorkScheduleMode��6`   $��Criação de Escala��6�   $��15(�6�    6�   �� *IsCreatingWorkSchedule+5      	� 5      -   5        $ ��RadioBtMergingWorkSchedule��5   "   $��Center=�6D   $	 PeriodicWorkScheduleMode��6_   $,��&Criação de Escala (compondo equipes)��6�   $��15(�6�   $��30 0 0 0q�6�    6�   �� *IsMergingWorkSchedule+5      	� 5      -   5        $!��RadioBtUpdatingWorkSchedule��5   "   $��Center=�6E   $	 PeriodicWorkScheduleMode��6`   $��Atualização de Escala��6�   $��15(�6�   $��30 0 0 0q�6�    6�   �� *IsUpdatingWorkSchedule+5      5      5      5      ]� 5   
   $��84��6   .+�5       �� $��	Arquivo: ��6   $
��Left=�6(   $��69��6>   $��32��6I   $
��0,26q�6U   $��15(�6c   �� 5!      
 �� 
IsReadOnly
 . $��	5,27,0,27q�6F   $	��680��6Y   $��30��6e   $��15(�6q   ��6(   �� *FullPathFile+6q   �� 5"      -   5         ��Name$ 	btnSearch��5"      $��Right=�6(      DefaultTextImageButton#��[  6\    � ToolTip$ 	Pesquisar��6�   $��52��6�   $	��0,3q�6�   .��5#      ]� $��2q�6   .+�5$      �� $E��?/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Search48x48.png��6   $��Right=�6e   $��30��6|   5%      5&      �� 5'      -	   5        $ btnValidate��5'      $
��Left=�6*   $��15(�6E   #��[  6S   $ Validar��6�   $��	40 0 20 0q�6�   $	��113��6�   .��5(      ]� $	��100��6   $��42��6(   $��5q�64   .+�5)      �� $A��;/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Refresh.png��6   $��Right=�6a   $��42��6x   $��Bottom=�6�   �� 5*      $��Bottom=�6    $
��Left=�6;   $��0,0,0,14q�6Q   �� 6d   $�Validar��6h   6z   5+      5,      5-      �� 5.   
   $��0,5,0,0q�6   $ 
Horizontal=�6&   .+�5/      &   System.Windows.Controls.DataGrid  -
   5          Name$ dataGridReportValidation��5/      
�������   
IsReadOnly .    GridLinesVisibility$
 None=�50   @   $	��Top=�51      $	��221��63      AutoGenerateColumns .  $	��450��52      ��5/   =   �� *RevortValidationList+   Columns 54      0  *System.Windows.Controls.DataGridTextColumn  55      ,  &System.Windows.Controls.DataGridColumn   Header$ Erros/alertas��6-      Width5  /System.Windows.Controls.DataGridLengthConverter$	 170 6E   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6Q   ��  & "ReportValidationToMessageConverter �� 	Converter# [    ElementStyle 56      �� 57   "   .�58   &   ��  �� Property � �� �� Value$
 Wrap=�6O   �� 59   &    � ��$ 15(�6K   5:   #   5<        5=      $  ��6-   $	 220 67    6C   �� *DataAffected+ 5>      �� 5?   "   .�5@   &   ��  � ��$
 Wrap=�6O   �� 5A   &    � ��$ 15(�6K   5B   #   5D      4  .System.Windows.Controls.DataGridTemplateColumn  5E      $ 30 61      
IsReadOnly . $  ��6N      CellTemplate 5F      �� 5G   "   .R�5H   &   �� ��6,   ��  ! ValidationLevelImageConverter# [ 6{   5I   #   5K      5O        5Q      -   5        $" dataGridPeriodicWorkSchedule��5Q      
������� . $
 None=�5R   @   $	��Top=�5S      $��30 0 0 0q�63   $	��221��6E   $	��450��6R    .  ��5Q   A   �� *.,WorkScheduleImported.WorkScheduleForUnitList+   RowStyle 5T      �� 5U       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6     �� Triggers 5V      �� 5W   "    �� Binding 6.   �� *
HasWarning+ �� Value True6M   .a�5X   &   ��  � ��
 � ���6C   5Y   #   �� 5Z   "    6.   �� *
HasError+ True6K   .a�5[   &   ��  � ��
 �  ��6C   5\   #   �� 5]   "    6.   �� *HasSelectedError+ True6S   .a�5^   &   ��  �@ 
IsSelected . 6C   5_   #   5a       5c        5d      $ Linha��6-    6=   �� *Row+  5e      $
 Data��6-    6<   �� *	ShiftDate+  �� StringFormat$  
dd/MM/yyyy��  5f      $ Turno��6-    6<   �� *WorkshiftLabel+  5g      $ AM��6-    6:   �� *UnitId+  5h      $
 Tipo��6-    6<   �� *
UnitType+  5i      $ Equipe��6-    6>   �� *ConcatenatedCrewMembers+5k      5l      ]� 5o   
   $��80��6   .+�5p      �� -   5        $ btnBack��5p      $
��Left=�6&   $��15(�6A   #��[  6e   $ Voltar��6�   .��5q      ]� $	��138��6   $��42��6(   $��5q�64   .+�5r      �� $>��8/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Exit.png��6   $��Right=�6^   �� 5s      $��Center=�6    $
��Left=�6;   �� 6S   $�Voltar��6W   6h   5t      5u      �� 5v      -   5        $ btnDownloadModel��5v      $��Right=�6/   $��15(�6K   #��[  6Z   $ Baixar Modelo��6�   $��10q�6�   .��5w      ]� $	��138��6   $��42��6(   $��5q�64   .+�5x      �� $C��=/Sisgraph.Ips.Samu.AddIn.Views;component/Images/SheetIcon.png��6   $��Right=�6c   $��42��6z   $	��Top=�6�   �� 5y      $��Bottom=�6    $
��Left=�6;   $
��Wrap=�6Q   �� 6f   $�Baixar XLS importação��6j   6�   5z      5{      �� 5|      ���#F1F4F7$��1q�6+   ���#FEFEFE$	��300��6T   $��80��6`   ._�5}      �� $��15q�6   ��5~      �� 5      " ��6     5�      �� 5�   "    6.   �� *	IsLoading+ True6L   .a�5�   &   ��  � y�$ Visible=�6C   5�   #   �� 5�   "    6.   �� *	IsLoading+	 False6L   .a�5�   &   ��  � y�$ Hidden=�6C   5�   #   5�      .+�5�      �� ��6    �� *LoadingText+6?   � 5�      ! � IsIndeterminate! . {�. $��24��6J   6X   5�      5�      �� 5�      -   5        $ 	btnImport��5�      $��Right=�6(   $��15(�6D   #��[  6j   $ Importar��6�   {�6�   �� *EnableImporting+.��5�      ]� $	��138��6   $��42��6(   $��5q�64   .+�5�      �� $@��:/Sisgraph.Ips.Samu.AddIn.Views;component/Images/Import.png��6   $��Right=�6`   �� 5�      $��Bottom=�6    $
��Left=�6;   $��0,0,0,14q�6Q   �� 6d   $�Importar��6h   6{   5�      5�      5�      5�      5�      