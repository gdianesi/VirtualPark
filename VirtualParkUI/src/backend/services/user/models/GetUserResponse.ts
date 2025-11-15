export interface GetUserResponse {
    id: string;
    name: string;
    lastName: string;
    email: string;
    roles: string[];
    visitorProfileId?: string;
}
