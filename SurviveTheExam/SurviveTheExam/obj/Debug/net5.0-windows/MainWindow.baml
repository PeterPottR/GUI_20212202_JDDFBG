   M S B A M L   `   `   `  ����I  ESurviveTheExam, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SurviveTheExamSurviveTheExam  5   	       SurviveTheExam.MainWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:SurviveTheExam     5   	     � WindowStartupLocation$  CenterScreen=�5   	    � Title$ 
MainWindow��5	   	   $	��800��6   $	��750��6)   .��5
      � ��5   
   �� 5       �� ImageSource$ 	/main.png��6    � ColumnDefinitions 5   
   �� 5      $��5* �6   �� 5      $��3* �6    � RowDefinitions 5   
   �� 5      $��6* �6   �� 5      $��2* �6   �� 5      $��1* �6   �� 5      $��2* �6   �� 5      $��2* �6   .+�5   
   �� -   5        $��1��5      $��1��6!   
��� E��$��50��6U   $��New game��6a   $��28(�6t   $��Comic Sans m)�6�   $��	0,40,53,0q�6�   �� 5   
   -   5        $��1��5      $��3��6!   
��� E��$��50��6Y   $��Leaderboard��6e   $��28(�6{   $��Comic Sans ms)�6�   $��	0,0,53,40q�6�   �� 5   
    �� Orientation$ 
Horizontal=�6   $��2��6.   $��1��6;   $	��Top=�6K   $��Center=�6c   .+�5      �� $��Enter your name: ��6   $��35��60   
��� E��$��18(�6S   $��Comic Sans m)�6a   �� 5      -   5         ��Name$ 
playerName��5      $��35��6*   $	��120��66    �� TextWrapping$
 Wrap=�6B   $��18(�6V   5      5      5       