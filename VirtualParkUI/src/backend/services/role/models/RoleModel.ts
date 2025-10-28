export interface RoleModel {
    id: string;
    name: string;
    description: string;
    permissionsIds: string[];
    usersIds: string[];
}