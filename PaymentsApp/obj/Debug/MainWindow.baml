   M S B A M L   `   `   `  ����F  BPaymentsApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PaymentsAppPaymentsApp  5   	       PaymentsApp.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:PaymentsApp     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � Title$  Платежи��5	   	   $	��500��6   $
��1020��6&   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      �� 5      $
��auto �6   .+�5   
   � ���#eee6   � 5   
   $��15q�6    � Orientation$ 
Horizontal=�5      $��Center=�5      .+�5      ��  �� Orientation$ 
Horizontal=�6   $��20 10q�5      .+�5      �� -   5         ��Name$ 	BtnAddPay��5      $��+��5         MaterialDesignFlatButton#��[  5      $��40��5      $��40��5      $��0 0 20 0q�5       �� 5"      -   5        $ BtnDeletePay��5"      $��-��5$      #��[  5%      $��40��5&      $��40��5'      5(      �� 5*      $ 
Horizontal=�6   $��Center=�5+      $��20 10q�5,      .+�5-      �� $
��С: ��6    �� TextAlignment$ Right=�5.      $��Center=�5/      (  "System.Windows.Controls.DatePicker  50      -   5          Name$ 
datePicker��50      $	��200��52      54      �� 56      $ 
Horizontal=�6   $��Center=�57      $��20 10q�58      .+�59      �� $��По: ��6   $ Right=�5:      $��Center=�5;        5<      -   5        $ datePicker2��5<      $	��200��5>      5@      �� 5B      -   5         ��Name$ 
ComboTypes��5B       �� DisplayMemberPath$ PaymentName��5D      $	��220��5E      $��20 10q�5F      5H      &   System.Windows.Controls.DataGrid  5J   
   -   5        	  Name$	 DGridPayments��5J      $��1��5K      
   AutoGenerateColumns
 .     
IsReadOnly .    Columns 5N      4  .System.Windows.Controls.DataGridCheckBoxColumn  5O      ,  &System.Windows.Controls.DataGridColumn   Header$ Оплачено��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 5P   )   �� *	IsPayed+0  *System.Windows.Controls.DataGridTextColumn  5Q      $- 'Наименование платежа��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 4* 5R   %    5S   %   �� *PaymentName+  5T      $ Количество��6%    5U   %   �� *Count+  5V      $ Цена��6%    5W   %   �� *Price+  5X      $ 
Сумма��6%   $ 3* 5Y   %    5Z   %   �� *Summ+  5[      $ Категория��6%   $ 4* 5\   %    5]   %   �� *PaymentType.PaymentName+4  .System.Windows.Controls.DataGridTemplateColumn  5^      $
 auto 6)      CellTemplate 5_      �� 5`      .R�5a      �� $ 
BtnEditPay��6%   -   5        $ ��Редактировать��5c   %   #��[  5d   %   5e      5g      5i      � 5k   
   $��2��6    � ColumnDefinitions 5l      �� 5m      �� 5n      $��20 �6#   �� 5o      �� 5p      $��20 �6#   �� 5q      .+�5t      �� -	   5        $ BtnUpdateGrid��5t      $%��Обновить таблицу��5v      $	��200��5w      $��Center=�5x         MaterialDesignOutlinedButton#��[ 5y      �� 5{      -
   5        $ BtnGetDiagramm��5|      $��2��5{      $4��.Получить график расходов��5~      $��Center=�5      #��[ 5�      �� 5�      -   5        $ BtnExit��5�      $��5��5�      $��
Выйти��5�      $	��200��5�      $��10q�5�      $��Center=�5�      5�      5�      5�      