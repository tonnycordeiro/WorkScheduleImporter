CREATE OR REPLACE FUNCTION SGPH_GET_WORKSHIFT_TABLE (DATE_STAMP_START VARCHAR2, DATE_STAMP_END VARCHAR2)
RETURN WORKSHIFT_TABLE_TYPE PIPELINED
IS
BEGIN

FOR RECORD_OUTPUT IN (
select '07-19' workshift, dt.COLUMN_VALUE workshiftDate, to_char(dt.COLUMN_VALUE,'YYYYMMDD') workshiftDateTS, to_char(to_date(substr(fn_datetime_to_string(dt.COLUMN_VALUE), 1, 8),
                                                  'YYYYMMDD') +
                                          (cast(substr('07-19', 1, 2) as int) -
                                           (case when exists 
                                                 (select summer_period_tzoffset x
                                                    from reporting.brazil_timezone
                                                    where state = 'SP'
                                                      and dt.COLUMN_VALUE  between SUMMER_PERIOD_LOCAL_START and SUMMER_PERIOD_LOCAL_END)
                                                 then -2 else -3 end)) / 24,
                                          'YYYYMMDDHH24MISS') || 'UT' workshiftStart,

                                  to_char(to_date(substr(fn_datetime_to_string(dt.COLUMN_VALUE), 1, 8),
                                                  'YYYYMMDD') +
                                          (cast(substr('07-19', 4, 2) as int) -
                                           (case when exists 
                                                 (select summer_period_tzoffset x
                                                    from reporting.brazil_timezone
                                                    where state = 'SP'
                                                      and dt.COLUMN_VALUE  between SUMMER_PERIOD_LOCAL_START and SUMMER_PERIOD_LOCAL_END)
                                                 then -2 else -3 end)) / 24 + (case when (cast(substr('07-19',4,2) as int) < cast(substr('07-19',1,2) as int)) then 1 else 0 end),
                                          'YYYYMMDDHH24MISS') || 'UT' workshiftEnd
                             from Dual cross join table(SGPH_GET_DATES(DATE_STAMP_START, DATE_STAMP_END)) dt
            union
                select '19-07' workshift, dt.COLUMN_VALUE workshiftDate, to_char(dt.COLUMN_VALUE,'YYYYMMDD') workshiftDateTS, to_char(to_date(substr(fn_datetime_to_string(dt.COLUMN_VALUE), 1, 8),
                                                  'YYYYMMDD') +
                                          (cast(substr('19-07', 1, 2) as int) -
                                           (case when exists 
                                                 (select summer_period_tzoffset x
                                                    from reporting.brazil_timezone
                                                    where state = 'SP'
                                                      and dt.COLUMN_VALUE  between SUMMER_PERIOD_LOCAL_START and SUMMER_PERIOD_LOCAL_END)
                                                 then -2 else -3 end)) / 24,
                                          'YYYYMMDDHH24MISS') || 'UT' workshiftStart,

                                  to_char(to_date(substr(fn_datetime_to_string(dt.COLUMN_VALUE), 1, 8),
                                                  'YYYYMMDD') +
                                          (cast(substr('19-07', 4, 2) as int) -
                                           (case when exists 
                                                 (select summer_period_tzoffset x
                                                    from reporting.brazil_timezone
                                                    where state = 'SP'
                                                      and dt.COLUMN_VALUE  between SUMMER_PERIOD_LOCAL_START and SUMMER_PERIOD_LOCAL_END)
                                                 then -2 else -3 end)) / 24 + (case when (cast(substr('19-07',4,2) as int) < cast(substr('19-07',1,2) as int)) then 1 else 0 end),
                                          'YYYYMMDDHH24MISS') || 'UT' workshiftEnd
                             from Dual cross join table(SGPH_GET_DATES(DATE_STAMP_START, DATE_STAMP_END)) dt
    )
    LOOP
        PIPE ROW (WORKSHIFT_DATA_TYPE(RECORD_OUTPUT.workshift, RECORD_OUTPUT.workshiftDate,RECORD_OUTPUT.workshiftDateTS,RECORD_OUTPUT.workshiftStart,RECORD_OUTPUT.workshiftEnd));
    END LOOP;

END;
