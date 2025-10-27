export type AttractionType = 'RollerCoaster' | 'Simulator' | 'Show';

export interface AttractionModel {
  id: string;
  name: string;
  type?: AttractionType;
  description?: string;
}

export interface CreateAttractionRequest {
  Name: string;
  Type: AttractionType;
  MiniumAge: string;
  Capacity: string;
  Description: string;
  Available: string;
}

export interface CreateAttractionResponse {
  id: string;
}

export interface GetAttractionResponse {
  Id: string;
  Name: string;
  Type: string;
  MiniumAge: string;
  Capacity: string;
  Description: string;
  EventIds: string[];
  Available: string;
}