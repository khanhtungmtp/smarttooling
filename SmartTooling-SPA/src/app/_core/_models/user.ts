export interface User {
    id: number;
    account: string;
    email: string;
    fullname: string;
    created_by: string;
    created_time: Date;
    updated_by: string;
    updated_time: Date;
    active: boolean;
    role: string;
}

export class AddUser {
    account: string = '';
    password: string = '';
    email: string = '';
    name: string = '';
    updated_by: string = '';
    updated_time: Date = new Date;
    is_active: boolean = true;
}
