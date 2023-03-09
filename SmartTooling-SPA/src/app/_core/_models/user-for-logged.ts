export interface UserForLogged {
    account: string;
    password: string;
    name: string;
    email: string;
    is_active: boolean;
    update_by: string;
    update_time: string | null;
}