use [##DatabaseName##]
GO

insert into GPSData(lSerial_ID, fLatitude, fLongitude, nSpeed, nBearing, tiNum_Sats, tiFix_Type, rHDOP, rPDOP, client_id, biPacketID, tiCell_Strength, event_date_gmt, receive_date_gmt, event_date_local, receive_date_local, point_color, fix_confidence_level, fix_confidence_threshold, was_processed, tiCurrent_Zone_State) values
(1100302, 49.187802, -122.984223, 0.0, 0.0, 4, 23, 16, NULL, 45397, 0, 3, '2008-12-20 00:00:26.000', '2008-12-20 00:04:03.170', '2008-12-19 17:00:26.000', '2008-12-19 17:04:03.170', 1, 0, 0, 1, 1)
insert into GPSData(lSerial_ID, fLatitude, fLongitude, nSpeed, nBearing, tiNum_Sats, tiFix_Type, rHDOP, rPDOP, client_id, biPacketID, tiCell_Strength, event_date_gmt, receive_date_gmt, event_date_local, receive_date_local, point_color, fix_confidence_level, fix_confidence_threshold, was_processed, tiCurrent_Zone_State) values
(1100302, 49.189674, -122.995552, 0.0, 202.0, 4, 23, 16, NULL, 45397, 0, 3, '2008-12-19 23:59:26.000', '2008-12-20 00:04:03.170', '2008-12-19 16:59:26.000', '2008-12-19 17:04:03.170', 1, 0, 0, 1, 1)
insert into GPSData(lSerial_ID, fLatitude, fLongitude, nSpeed, nBearing, tiNum_Sats, tiFix_Type, rHDOP, rPDOP, client_id, biPacketID, tiCell_Strength, event_date_gmt, receive_date_gmt, event_date_local, receive_date_local, point_color, fix_confidence_level, fix_confidence_threshold, was_processed, tiCurrent_Zone_State) values
(1100302, 49.185957, -122.979127, 0.0, 0.0, 4, 23, 19, NULL, 45397, 0, 3, '2008-12-19 23:57:45.000', '2008-12-20 00:04:03.153', '2008-12-19 16:57:45.000', '2008-12-19 17:04:03.153', 1, 0, 0, 1, 1)
insert into GPSData(lSerial_ID, fLatitude, fLongitude, nSpeed, nBearing, tiNum_Sats, tiFix_Type, rHDOP, rPDOP, client_id, biPacketID, tiCell_Strength, event_date_gmt, receive_date_gmt, event_date_local, receive_date_local, point_color, fix_confidence_level, fix_confidence_threshold, was_processed, tiCurrent_Zone_State) values
(1100302, 49.187019, -122.975404, 0.0, 0.0, 4, 23, 17, NULL, 45397, 0, 3, '2008-12-20 00:00:56.000', '2008-12-20 00:04:03.170', '2008-12-19 17:00:56.000', '2008-12-19 17:04:03.170', 1, 0, 0, 1, 1)
insert into GPSData(lSerial_ID, fLatitude, fLongitude, nSpeed, nBearing, tiNum_Sats, tiFix_Type, rHDOP, rPDOP, client_id, biPacketID, tiCell_Strength, event_date_gmt, receive_date_gmt, event_date_local, receive_date_local, point_color, fix_confidence_level, fix_confidence_threshold, was_processed, tiCurrent_Zone_State) values
(1100302, 49.192008, -122.994426, 0.0, 0.0, 4, 23, 18, NULL, 45397, 0, 3, '2008-12-20 00:25:15.000', '2008-12-20 00:31:14.893', '2008-12-19 17:25:15.000', '2008-12-19 17:31:14.893', 1, 0, 0, 1, 1)

insert into tbldevice(ldevice_id, seid, sdevice_description, bdevice_active, client_id, ldevice_type_id, agency_id, dassigned, status, dmodified, change_user_id, dtCallBack, dStatus_date, bAgencyOwned, case_id, is_voice_enabled, service_plan_id, rlc_timestamp, calibration_due_date) values
(114503, 1100302, NULL, 1, 45397, 13, 179, '2008-12-29 23:13:41.890', 'A' , '2008-12-29 23:13:41.890', 0, '2010-06-08 20:34:49.033', '2008-12-29 23:13:41.890', 0, 44558, 0, 16, NULL, NULL)

insert into tbldevice(ldevice_id, seid, sdevice_description, bdevice_active, client_id, ldevice_type_id, agency_id, dassigned, status, dmodified, change_user_id, dtCallBack, dStatus_date, bAgencyOwned, case_id, is_voice_enabled, service_plan_id, rlc_timestamp, calibration_due_date) values
(114525, 1100325, NULL, 1, 45421, 13, 179, '2008-12-29 20:53:27.000', 'A' , '2008-12-29 20:53:27.000', 0, '2009-04-09 19:42:20.007', '2008-12-29 20:53:27.000', 0, 44582, 0, 16, NULL, NULL)

insert into client(client_id, entity_type_id, is_active, last_name, first_name, middle_initial, ssn, birth_date, gender, alias, comment, inactivate_reason, create_date, change_date, change_user_id, case_number, start_date, end_date, risk_level, default_language_id, height, weight, client_type_id, country_code_of_origin, spoken_language_id, EducationLevelId, EnglishSpokenProficiency, EnglishWrittenProficiency) values
(45397, 4, 1, 1100302, 'Beta - Battery Test', '', '', '1800-01-01 00:00:00.000', 'M', '', '', '', '2008-12-18 00:36:33.740', '2008-12-30 14:46:25.900', 7719, '', '2008-12-17 00:00:00.000', '2009-12-17 00:00:00.000', 2, 53, 0, 0, 0, NULL, NULL, NULL, 0, 0)

insert into client(client_id, entity_type_id, is_active, last_name, first_name, middle_initial, ssn, birth_date, gender, alias, comment, inactivate_reason, create_date, change_date, change_user_id, case_number, start_date, end_date, risk_level, default_language_id, height, weight, client_type_id, country_code_of_origin, spoken_language_id, EducationLevelId, EnglishSpokenProficiency, EnglishWrittenProficiency) values
(45421, 4, 1, 1100325, 'Beta - At Sendum', '', '', '1800-01-01 00:00:00.000', 'M', '', '', '', '2008-12-18 01:46:29.653', '2009-01-02 20:09:17.783', 7719, '', '2008-12-17 00:00:00.000', '2009-12-17 00:00:00.000', 2, 53, 0, 0, 0, NULL, NULL, NULL, 0, 0)

insert into agency(agency_id, entity_type_id, is_active, agency_name, customer_number, agency_type, selfpay_setting, contact_last_name, contact_first_name, create_date, change_date, change_user_id, can_fax_alerts, customer_id, Agency_Password, service_level, appUser_password_expiration_days, default_language_id, agency_group_id, inclusion_grace_period_default, rf_grace_period_default, terms_id, CustomerGroupId) values
(179, 5, 1, 'zy_Testing Agency', 98765432, 0, 'B', 'Brugel', 'Sirry', '2005-09-14 06:44:16.467', '2011-09-08 18:39:48.777', 13327, 1, NULL, 'B75C1E09-A8BC-4276-8DE1-80EC14C432DA', 0, 90, 53, 0, 15, 0, NULL, NULL)

insert into address (entity_id, entity_type_id, address1, address2, city, state, country_code, time_zone_id, create_date, change_date, change_user_id, postal_code, AddressTypeID, SequenceID) values
(45397, 4, '6400 lookout rd', '', 'boulder', 'CO', 840, 3, '2008-12-18 00:36:33.753', '2008-12-18 00:36:33.753', 7719, '80301', 1, 1)

insert into address (entity_id, entity_type_id, address1, address2, city, state, country_code, time_zone_id, create_date, change_date, change_user_id, postal_code, AddressTypeID, SequenceID) values
(45421, 4, '6400 lookout rd', '', 'boulder', 'CO', 840, 3, '2008-12-18 01:46:29.653', '2008-12-18 01:46:29.653', 7719, '80301', 1, 1)