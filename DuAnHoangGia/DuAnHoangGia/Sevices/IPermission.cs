using System;
using System.Collections.Generic;
using System.Text;

namespace DuAnHoangGia.Sevices
{
    public enum Permission
    {
        //
        // Summary:
        //     The unknown permission only used for return type, never requested
        Unknown = 0,
        //
        // Summary:
        //     Android: Calendar iOS: Calendar (Events) UWP: None
        Calendar = 1,
        //
        // Summary:
        //     Android: Camera iOS: Photos (Camera Roll and Camera) UWP: None
        Camera = 2,
        //
        // Summary:
        //     Android: Contacts iOS: AddressBook UWP: ContactManager
        Contacts = 3,
        //
        // Summary:
        //     Android: Fine and Coarse Location iOS: CoreLocation (Always and WhenInUse) UWP:
        //     Geolocator
        Location = 4,
        //
        // Summary:
        //     Android: Microphone iOS: Microphone UWP: None
        Microphone = 5,
        //
        // Summary:
        //     Android: Phone iOS: Nothing
        Phone = 6,
        //
        // Summary:
        //     Android: Nothing iOS: Photos UWP: None
        Photos = 7,
        //
        // Summary:
        //     Android: Nothing iOS: Reminders UWP: None
        Reminders = 8,
        //
        // Summary:
        //     Android: Body Sensors iOS: CoreMotion UWP: DeviceAccessInformation
        Sensors = 9,
        //
        // Summary:
        //     Android: Sms iOS: Nothing UWP: None
        Sms = 10,
        //
        // Summary:
        //     Android: External Storage iOS: Nothing
        Storage = 11,
        //
        // Summary:
        //     Android: Microphone iOS: Speech UWP: None
        Speech = 12,
        //
        // Summary:
        //     Android: Fine and Coarse Location iOS: CoreLocation - Always UWP: Geolocator
        LocationAlways = 13,
        //
        // Summary:
        //     Android: Fine and Coarse Location iOS: CoreLocation - WhenInUse UWP: Geolocator
        LocationWhenInUse = 14,
        //
        // Summary:
        //     Android: None iOS: MPMediaLibrary UWP: None
        MediaLibrary = 15
    }
    public interface IPermission
    {
        void RequestPermission(params Permission[] permissions);
        bool CheckSelfPermission(Permission permission);
     }
}
