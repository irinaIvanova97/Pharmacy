   M S B A M L   `   `   `  ����C  ?Pharmacy, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Pharmacy.DealersPharmacy.Dealers  5   	   5clr-namespace:Pharmacy.ControlsPharmacy.Controls  5   	   $    Pharmacy.Dealers.DealersDialog   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:Pharmacy.Dealers     5   	   6
b_controlsclr-namespace:Pharmacy.Controls     5   	     � Title$  DealersDialog��5	   	   $	��334��6   $	��700��6,   $	��650��68   .��5
      � .+�5   
   �� $��	* Име:��6   $
��Left=�6!   $	��Top=�6<   $��0,7,0,0q�6T   �� 5   
   -   5         ��Name$ 
ui_edbName��5      $
��Left=�6&   $��23��6A   $��
125,10,0,0q�6M    �� TextWrapping$
 Wrap=�6a   $	��Top=�6u   $	��130��6�   ��.  6�   �� 5   
   $��* Дистрибутор:��6   $
��Left=�6)   $	��Top=�6D   $��0,45,0,0q�6\   �� 5   
   -   5        $ ui_edbDistrubutor��5      $
��Left=�6-   $��23��6H   $��
125,48,0,0q�6T   $
 Wrap=�6h   $	��Top=�6|   $	��130��6�   ��.  6�   �� 5   
   -   5         ��Name$ ui_btnOK��5      $��OK��6#   $��Right=�60   $��	0,0,90,25q�6L   $��Bottom=�6_   $��75��6z   ��.  6�   �� 5   
   -   5        $ ui_btnCancel��5      $��Cancel��6'   $��Right=�68   $��	0,0,10,25q�6T   $��Bottom=�6g   $��75��6�   ��.  6�   �� 5   
   $&�� * Телефонен номер:��6   $
��Left=�6-   $	��Top=�6H   $��	270,6,0,0q�6`   �� 5   
   -   5        $ ui_edbPhoneNumber��5      $
��Left=�6-   $��23��6H   $��
394,10,0,0q�6T   $
 Wrap=�6h   $	��Top=�6|   $	��130��6�   ��.  6�   �� 5   
   $��Лекарства��6   $��10,94,10,68q�6&   .��5      �� ��.  � UseLayoutRounding .  $
��Auto=�6M   $��Disabled=�6r   $��2,0,-2,0q�6�   .��5      � -   5        $��ui_gridDrugs��5      .+�5      �� -   5         ��Name$ ui_lblNoDrugs��5      $!��Няма лекарства��63   $��Center=�6L   $��0q�6i   $��Center=�6t   5      5      5      �� 5   
   -   5        $ ui_btndrugExpiryDate��5      $8��2Проверка за срок на годност��6/   $��	20,0,0,25q�6U   $��Bottom=�6h   $	��177��6�   $
��Left=�6�   �� 5   
   -	   5        $ ui_btnSortByPrice��5      $&�� Сортиране по цена��6,   $
��Left=�6H   $��
202,0,0,25q�6c   $��Bottom=�6w   $	��126��6�   6�   �� 5   
   -
   5        $ ui_btnSortByNumber��5      $&�� Сортиране по брой��6-   $
��Left=�6I   $��
333,0,0,25q�6d   $��Bottom=�6x   $	��126��6�   6�   5      5      