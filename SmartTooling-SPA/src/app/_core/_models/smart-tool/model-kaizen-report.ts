export interface ModelKaizenReport {
    factory_id: string;
    model_no: string;
    model_name: string;
    model_type_id: string;
    model_family: string;
    upper_id: string;
    dev_season: string;
    prod_season: string;
    volume: number;
    volume_string: string;
    volume_percent: number;
    remarks: string;
    model_picture: string;
    is_active: boolean;
}