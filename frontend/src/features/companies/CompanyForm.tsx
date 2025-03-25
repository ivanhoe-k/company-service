import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import {
  Company,
  createCompany,
  updateCompany,
  fetchCompanyById
} from '../../api/companies';

function CompanyForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEditing = !!id;

  const [form, setForm] = useState<{
    name?: string;
    exchangeName?: string;
    ticker?: string;
    website?: string;
    isin?: string; // flat for easier handling
  }>({});

  useEffect(() => {
    if (isEditing) {
      fetchCompanyById(id!).then((company) => {
        setForm({
          name: company.name,
          exchangeName: company.exchange,
          ticker: company.ticker,
          website: company.website
          // no isin for edit
        });
      });
    }
  }, [id, isEditing]);

  const handleChange = (field: string, value: string) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (isEditing) {
        await updateCompany(id!, {
          name: form.name,
          exchangeName: form.exchangeName,
          ticker: form.ticker,
          website: form.website
        });
      } else {
        await createCompany({
          name: form.name,
          exchangeName: form.exchangeName,
          ticker: form.ticker,
          website: form.website,
          isin: { value: form.isin }
        });
      }
      navigate('/companies');
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <div className="p-6">
      <h1 className="text-xl font-bold mb-4">{isEditing ? 'Edit' : 'Create'} Company</h1>
      <form onSubmit={handleSubmit} className="space-y-4 max-w-md">
        <input
          type="text"
          placeholder="Name"
          value={form.name || ''}
          onChange={(e) => handleChange('name', e.target.value)}
          className="w-full p-2 border rounded"
        />
        <input
          type="text"
          placeholder="Exchange"
          value={form.exchangeName || ''}
          onChange={(e) => handleChange('exchangeName', e.target.value)}
          className="w-full p-2 border rounded"
        />
        <input
          type="text"
          placeholder="Ticker"
          value={form.ticker || ''}
          onChange={(e) => handleChange('ticker', e.target.value)}
          className="w-full p-2 border rounded"
        />
        {!isEditing && (
          <input
            type="text"
            placeholder="ISIN"
            value={form.isin || ''}
            onChange={(e) => handleChange('isin', e.target.value)}
            className="w-full p-2 border rounded"
          />
        )}
        <input
          type="text"
          placeholder="Website"
          value={form.website || ''}
          onChange={(e) => handleChange('website', e.target.value)}
          className="w-full p-2 border rounded"
        />
        <button type="submit" className="bg-blue-600 text-white px-4 py-2 rounded">
          {isEditing ? 'Update' : 'Create'}
        </button>
      </form>
    </div>
  );
}

export default CompanyForm;
