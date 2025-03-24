import axios from './axios';

export interface Company {
  id: string;
  name?: string;
  exchange?: string;
  ticker?: string;
  isin?: string;
  website?: string;
}

export interface CreateCompanyRequest {
  name?: string;
  exchangeName?: string;
  ticker?: string;
  website?: string;
  isin: {
    value?: string;
  };
}

export interface UpdateCompanyRequest {
  name?: string;
  exchangeName?: string;
  ticker?: string;
  website?: string;
}

export interface PageInfo {
  currentPage: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

export interface CompanyPage {
  items: Company[];
  totalCount: number;
  pageInfo: PageInfo;
}

export const fetchCompanies = async (page = 1, size = 5): Promise<CompanyPage> => {
  const res = await axios.get('/companies', {
    params: {
      PageNumber: page,
      PageSize: size
    }
  });
  return res.data;
};

export const fetchCompanyById = async (id: string): Promise<Company> => {
  const res = await axios.get(`/companies/${id}`);
  return res.data;
};

export const createCompany = async (company: CreateCompanyRequest): Promise<Company> => {
  const res = await axios.post('/companies', company);
  return res.data;
};

export const updateCompany = async (
  id: string,
  company: UpdateCompanyRequest
): Promise<Company> => {
  const res = await axios.put(`/companies/${id}`, company);
  return res.data;
};
