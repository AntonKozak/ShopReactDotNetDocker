'use server';

import { Auction, PagedResult } from '@/app/types';
import { FieldValues } from 'react-hook-form';
import { fetchWrapper } from '../lib/fetchWrapper';

export async function getData(query: string): Promise<PagedResult<Auction>> {
  return fetchWrapper.get(`search?${query}`);
}

export async function getDetailedViewData(id: string) {
  return fetchWrapper.get(`auctions/${id}`);
}

export async function createAuction(data: FieldValues) {
  return fetchWrapper.post('auctions', data);
}

export async function updateAuction(data: FieldValues, id: string) {
  return fetchWrapper.put(`auctions/${id}`, data);
}

export async function deleteAuction(id: string) {
  return fetchWrapper.del(`auctions/${id}`);
}
