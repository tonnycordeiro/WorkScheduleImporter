   M S B A M L   `   `   `  ���� [  WWorkScheduleImporter.AddIn.Views, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:WorkScheduleImporter.AddIn.Views WorkScheduleImporter.AddIn.Views  L    FWorkScheduleImporter.AddIn.Views.UnitForceMap.ImportWorkScheduleWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @local.clr-namespace:WorkScheduleImporter.AddIn.Views     
�������3   -WorkScheduleImporter.AddIn.Views.ViewResource$    IMPORTING_DIALOG_WINDOW_TITLE � Title# Z   � SizeToContent$ Height=�$��1000.377�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�$	��600�� � 	Resources  �� Source�� $ ../DefaultResources.xaml?�%    .���� $��5,0,5,10q�.+��� "   IMPORTING_DIALOG_MAIN_TITLE#��Z $��Center=�$
��Bold"�$��18(��� -   $��5,0,5,10q�$��15(� d� 	IsChecked �� *IsPeriodicWorkSchedule+.���� .	   'CHECK_BOX_CREATE_PERIODIC_WORK_SCHEDULE#��Z	 �� -   $��GroupPeriodicWorkSchedule��'
    PERIODIC_WORK_SCHEDULE_GROUB_BOX#��Z
 $��5,0,5,10q�.���� .+��� $��5,0,5,10q� �� Orientation$ 
Horizontal=�.+��� *   #PERIODIC_WORK_SCHEDULE_INITIAL_DATE#��Z $
��Left=�$��90��$��32��$��15(�(  "System.Windows.Controls.DatePicker  -   $��StartDatePicker��$	��160��$��25��   SelectedDate �� *	StartDate+�� (   !PERIODIC_WORK_SCHEDULE_FINAL_DATE#��Z $
��Left=�$��90��$��32��$��30 0 0 0q�$��15(�  -   $��EndDatePicker��$	��160��$��25�� �� *	EndDate+�� $ 
Horizontal=�$��5,0,5,10q�.+��� ,   %PERIODIC_WORK_SCHEDULE_IMPORTING_TYPE#��Z $
��Left=�$	��150��$��32��$��15(�	� -   $!��RadioBtCreatingWorkSchedule��$��Center=� 	� 	GroupName$ PeriodicWorkScheduleMode��5   .PERIODIC_WORK_SCHEDULE_IMPORTING_TYPE_CREATING#��Z $��15(� �� *IsCreatingWorkSchedule+	� -   $ ��RadioBtMergingWorkSchedule��$��Center=�$ PeriodicWorkScheduleMode��4   -PERIODIC_WORK_SCHEDULE_IMPORTING_TYPE_MERGING#��Z $��15(�$��30 0 0 0q� �� *IsMergingWorkSchedule+	� -   $!��RadioBtUpdatingWorkSchedule��$��Center=�$ PeriodicWorkScheduleMode��5   .PERIODIC_WORK_SCHEDULE_IMPORTING_TYPE_UPDATING#��Z $��15(�$��30 0 0 0q� �� *IsUpdatingWorkSchedule+]� $��84��.+���    
FILE_LABEL#��Z $
��Left=�$��69��$��32��$
��0,26q�$��15(���  �� 
IsReadOnly . $��	5,27,0,27q�$	��680��$��30��$��15(����� *FullPathFile+�� -    ��Name$ 	btnSearch��$��Right=�   DefaultTextImageButton#��[   � ToolTip$ 	Pesquisar��$��52��$	��0,3q�.��]� $��2q�.+��� $H��B/WorkScheduleImporter.AddIn.Views;component/Images/Search48x48.png��$��Right=�$��30���� -	   $ btnValidate��$
��Left=�$��15(�#��[  $ Validar��$��	40 0 20 0q�$	��113��.��]� $	��100��$��42��$��5q�.+��� $D��>/WorkScheduleImporter.AddIn.Views;component/Images/Refresh.png��$��Right=�$��42��$��Bottom=��� $��Bottom=�$
��Left=�$��0,0,0,14q���    BUTTON_VALIDATE#�Z �� $��0,5,0,0q�$ 
Horizontal=�.+�&   System.Windows.Controls.DataGrid  -
     Name$ dataGridReportValidation��
�������   
IsReadOnly .    GridLinesVisibility$
 None=�$	��Top=�$	��221��   AutoGenerateColumns .  $	��450������ *RevortValidationList+   Columns 0  *System.Windows.Controls.DataGridTextColumn      COLUMN_WARNING_AND_ERRORS,  &System.Windows.Controls.DataGridColumn   Header# Z     Width5  /System.Windows.Controls.DataGridLengthConverter$	  170 1  +System.Windows.Controls.DataGridBoundColumn!   Binding! ��  & "ReportValidationToMessageConverter" �� 	Converter#" [ #   ElementStyle# �� .��� $ �� Property$ � ��% �� Value$
% Wrap=��� $ � ��$% 15(�  $  ��$	  220 ! �� *DataAffected+# �� .��� $ � ��$
% Wrap=��� $ � ��$% 15(�4  .System.Windows.Controls.DataGridTemplateColumn  $  30 &   
IsReadOnly& . $  ��'   CellTemplate' �� .R��� ����  ! ValidationLevelImageConverter#" [   -   $" dataGridPeriodicWorkSchedule��
������� . $
 None=�$	��Top=�$��30 0 0 0q�$	��221��$	��450�� .  ���� *.,WorkScheduleImported.WorkScheduleForUnitList+(   RowStyle( �� ) �� 
TargetType)	  #System.Windows.Controls.DataGridRow") 	 * �� Triggers* �� + �� Binding+ �� *
HasWarning+, �� Value, True.a��� $ � ��
% � ����� + �� *
HasError+, True.a��� $ � ��
% �  ���� + �� *HasSelectedError+, True.a��� $ �@	 
IsSelected% .    -   
COLUMN_ROW# Z- ! �� *Row+  .   COLUMN_DATE# Z. ! �� *	ShiftDate+/ �� StringFormat$/ 
dd/MM/yyyy��  0   COLUMN_SHIFT# Z0 ! �� *WorkshiftLabel+  1   COLUMN_UNIT# Z1 ! �� *UnitId+  2   COLUMN_TYPE# Z2 ! �� *
UnitType+  3   COLUMN_CREW# Z3 ! �� *ConcatenatedCrewMembers+]� $��80��.+��� -   $ btnBack��$
��Left=�$��15(�#��[  $ Voltar��.��]� $	��138��$��42��$��5q�.+��� $A��;/WorkScheduleImporter.AddIn.Views;component/Images/Exit.png��$��Right=��� $��Center=�$
��Left=��� 4   BUTTON_GO_BACK#�Z4 �� -   $ btnDownloadModel��$��Right=�$��15(�#��[  5   BUTTON_DOWNLOAD_TOOL_TIP# Z5 $��10q�.��]� $	��138��$��42��$��5q�.+��� $F��@/WorkScheduleImporter.AddIn.Views;component/Images/SheetIcon.png��$��Right=�$��42��$	��Top=��� $��Bottom=�$
��Left=�$
��Wrap=��� 6   BUTTON_DOWNLOAD#�Z6 �� ���#F1F4F7$��1q����#FEFEFE$	��300��$��80��._��� $��15q����� ") ��* �� + �� *	IsLoading+, True.a��� $ � y�$% Visible=��� + �� *	IsLoading+	, False.a��� $ � y�$% Hidden=�.+��� ���� *LoadingText+� 7 � IsIndeterminate7 . {�. $��24���� -   $ 	btnImport��$��Right=�$��15(�#��[  8   BUTTON_IMPORT_TOOL_TIP# Z8 {��� *EnableImporting+.��]� $	��138��$��42��$��5q�.+��� $C��=/WorkScheduleImporter.AddIn.Views;component/Images/Import.png��$��Right=��� $��Bottom=�$
��Left=�$��0,0,0,14q��� 9   BUTTON_IMPORT#�Z9 