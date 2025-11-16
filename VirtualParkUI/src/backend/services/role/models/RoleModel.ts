export interface RoleModel {
    id: string;
    name: string;
    description: string;
    permissionIds: string[];
    usersIds: string[];
}