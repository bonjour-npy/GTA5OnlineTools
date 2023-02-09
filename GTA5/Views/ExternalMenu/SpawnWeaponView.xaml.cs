﻿using GTA5.Data;

using GTA5Core.Client;
using GTA5Core.Feature;
using GTA5Core.Settings;
using GTA5Core.RAGE;
using GTA5Core.RAGE.Weapons;

namespace GTA5.Views.ExternalMenu;

/// <summary>
/// SpawnWeaponView.xaml 的交互逻辑
/// </summary>
public partial class SpawnWeaponView : UserControl
{
    public SpawnWeaponView()
    {
        InitializeComponent();
        this.DataContext = this;
        ExternalMenuWindow.WindowClosingEvent += ExternalMenuWindow_WindowClosingEvent;

        // 武器列表
        foreach (var wType in WeaponHash.WeaponTypes)
        {
            ComboBox_WeaponTypes.Items.Add(new IconMenu()
            {
                Icon = "\xe610",
                Title = wType.Key
            });
        }
        ComboBox_WeaponTypes.SelectedIndex = 0;

        // 子弹类型
        foreach (var item in MiscData.ImpactExplosions)
        {
            ComboBox_ImpactExplosion.Items.Add(item.Name);
        }
    }

    private void ExternalMenuWindow_WindowClosingEvent()
    {

    }

    private void ComboBox_WeaponTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        lock (this)
        {
            var index = ComboBox_WeaponTypes.SelectedIndex;
            if (index != -1)
            {
                ListBox_WeaponInfo.Items.Clear();

                Task.Run(() =>
                {
                    var typeName = WeaponHash.WeaponTypes.ElementAt(index).Key;
                    foreach (var item in WeaponHash.WeaponTypes[typeName])
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, () =>
                        {
                            if (index == ComboBox_WeaponTypes.SelectedIndex)
                            {
                                ListBox_WeaponInfo.Items.Add(new ModelInfo()
                                {
                                    Name = item.Key,
                                    DisplayName = item.Value,
                                    PreviewImage = RAGEHelper.GetWeaponImage(item.Key)
                                });
                            }
                        });
                    }
                });

                ListBox_WeaponInfo.SelectedIndex = 0;
            }
        }
    }

    private void Button_SpawnWeapon_Click(object sender, RoutedEventArgs e)
    {
        if (ListBox_WeaponInfo.SelectedItem is ModelInfo info)
        {
            Hacks.CreateAmbientPickup($"pickup_{info.Name}");
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////

    private void RadioButton_AmmoModifier_None_Click(object sender, RoutedEventArgs e)
    {
        if (RadioButton_AmmoModifier_None.IsChecked == true)
        {
            MenuSetting.Weapon.AmmoModifierFlag = 0;
            Weapon.AmmoModifier(0);
        }
        else if (RadioButton_AmmoModifier_AMMO.IsChecked == true)
        {
            MenuSetting.Weapon.AmmoModifierFlag = 1;
            Weapon.AmmoModifier(1);
        }
        else if (RadioButton_AmmoModifier_CLIP.IsChecked == true)
        {
            MenuSetting.Weapon.AmmoModifierFlag = 2;
            Weapon.AmmoModifier(2);
        }
        else if (RadioButton_AmmoModifier_Both.IsChecked == true)
        {
            MenuSetting.Weapon.AmmoModifierFlag = 3;
            Weapon.AmmoModifier(3);
        }
    }

    private void CheckBox_ReloadMult_Click(object sender, RoutedEventArgs e)
    {
        Weapon.ReloadMult(CheckBox_ReloadMult.IsChecked == true);
    }

    private void Button_NoRecoil_Click(object sender, RoutedEventArgs e)
    {
        Weapon.NoRecoil();
    }

    private void CheckBox_NoSpread_Click(object sender, RoutedEventArgs e)
    {
        Weapon.NoSpread();
    }

    private void CheckBox_Range_Click(object sender, RoutedEventArgs e)
    {
        Weapon.Range();
    }

    private void ComboBox_ImpactExplosion_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = ComboBox_ImpactExplosion.SelectedIndex;
        if (index != -1)
        {
            if (index == 0)
                Weapon.ImpactType(3);
            else
                Weapon.ImpactType(5);

            Weapon.ImpactExplosion(MiscData.ImpactExplosions[index].ID);
        }
    }

    private void Button_FillCurrentAmmo_Click(object sender, RoutedEventArgs e)
    {
        Weapon.FillCurrentAmmo();
    }

    private void Button_FillAllAmmo_Click(object sender, RoutedEventArgs e)
    {
        Weapon.FillAllAmmo();
    }
}
