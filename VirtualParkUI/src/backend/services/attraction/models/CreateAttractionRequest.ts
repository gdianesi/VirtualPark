import { AttractionType } from './AttractionType';

export interface CreateAttractionRequest {
  Name: string;
  Type: AttractionType;
  MiniumAge: string;
  Capacity: string;
  Description: string;
  Available: string;
}
