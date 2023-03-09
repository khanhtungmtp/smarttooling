export class MessageConstants {
  public static LOGGED_IN = 'Welcome to the system';
  public static LOGIN_FAILED = 'Your username and/or password do not match';

  public static LOGGED_OUT = 'You have been logged out';

  public static CONFIRM_DELETE = 'Are you sure delete?';
  public static CONFIRM_DELETE_MSG = 'Are you sure you want to delete this record?';
  public static CONFIRM_DELETE_RANGE_MSG = 'Are you sure you want to delete these records?';

  public static CREATED_OK_MSG = 'Create successfully';
  public static UPDATED_OK_MSG = 'Update successfully';
  public static DELETED_OK_MSG = 'Delete successfully';
  public static UPLOAD_OK_MSG = 'Upload successful';

  public static CREATED_ERROR_MSG = 'Creating failed on save';
  public static UPDATED_ERROR_MSG = 'Updating failed on save';
  public static DELETED_ERROR_MSG = 'Deleting failed on save';
  public static UPLOAD_ERROR_MSG = 'Uploading failed on save';

  public static UNAUTHORIZED = 'You are not authorized to view this page';

  public static UN_KNOWN_ERROR = 'Oops! Sorry, an error occurred while processing your request';
  public static SYSTEM_ERROR_MSG = 'An error occurred while connecting to the server';

  public static NO_DATA = 'No Data!';
  public static DATA_FOUND = 'Data Not Found!';
  public static DATA_EXIST = 'Data was existed!';

  public static INVALID_FILE = 'Please select a file';
  public static ALLOW_IMAGE_FILE = 'Allowed file extensions are .jpg, .png, .jpeg';
  public static ALLOW_VIDEO_FILE = 'Allowed file extensions are .mp4';
  public static FILE_IMAGE_SIZE = 'File size must be 5MB or smaller';
  public static FILE_VIDEO_SIZE = 'File size must be 20MB or smaller';
  public static FILE_NOT_FOUND = 'File not found';
  public static FILE_IS_NULL_OR_EMPTY = 'File is null or empty';

  public static ALLOW_EXCEL_FILE = 'Please select the Excel file format';
  public static DATA_UPLOAD_FAIL = 'Data upload failed';

  public static SELECT_DATE = 'Please select From Date and To Date';
  public static SELECT_FORMAT_DATE = 'Please enter the date in format "YYYY/MM/DD"';
  public static COMPARE_DATE = 'From Date cannot be greater than To Date';

  public static SELECT_RECORD = 'Please select at least 1 record to delete!';

  public static QUERY_SUCCESS = 'Query successfully';
  public static SELECT_ALL_QUERY_OPTION = 'Please select all query options';
  public static SELECT_QUERY_OPTIONS = 'Please select the necessary options (*) to query';

  public static CLEAR = 'Clear successfully';
  public static CLEAR_ALL = 'please check again';
}

export class CaptionConstants {
  public static LOGIN_FAILED = 'Login Failed!';
  public static SUCCESS = 'Success!';
  public static ERROR = 'Error!';
  public static WARNING = 'Warning!';
  public static UNAUTHORIZED = 'Unauthorized!';
}

export class ActionConstants {
  public static CREATE = 'CREATE';
  public static EDIT = 'EDIT';
  public static DELETE = 'DELETE';
  public static VIEW = 'VIEW';
  public static APPROVAL = 'APPROVAL';
  public static EXCEL_EXPORT = 'EXCEL_EXPORT';
  public static EXCEL_IMPORT = 'EXCEL_IMPORT';
  public static PRINT = 'PRINT';
  public static DONE = 'DONE';
  public static FINISH = 'FINISH';
  public static RELEASE = 'RELEASE';
}