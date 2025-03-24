import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { fetchCompanies, Company } from '../../api/companies';

function CompanyListPage() {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  useEffect(() => {
    setLoading(true);
    fetchCompanies(page)
      .then((data) => {
        setCompanies(data.items || []);
        setTotalPages(data.pageInfo?.totalPages || 1);
      })
      .finally(() => setLoading(false));
  }, [page]);

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-semibold">Companies</h1>
        <Link
          to="/companies/new"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          + New Company
        </Link>
      </div>

      {loading ? (
        <p>Loading...</p>
      ) : (
        <>
          <table className="w-full table-auto border">
            <thead>
              <tr className="bg-gray-100">
                <th className="border p-2 text-left">Name</th>
                <th className="border p-2 text-left">Ticker</th>
                <th className="border p-2 text-left">Exchange</th>
                <th className="border p-2 text-left">ISIN</th>
                <th className="border p-2 text-left">Website</th>
                <th className="border p-2 text-left">Actions</th>
              </tr>
            </thead>
            <tbody>
              {companies.map((company) => (
                <tr key={company.id}>
                  <td className="border p-2">{company.name}</td>
                  <td className="border p-2">{company.ticker}</td>
                  <td className="border p-2">{company.exchange}</td>
                  <td className="border p-2">{company.isin}</td>
                  <td className="border p-2">{company.website}</td>
                  <td className="border p-2">
                    <Link
                      to={`/companies/${company.id}/edit`}
                      className="text-blue-600 hover:underline"
                    >
                      Edit
                    </Link>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className="flex justify-center mt-4 gap-4">
            <button
              onClick={() => setPage((prev) => Math.max(prev - 1, 1))}
              disabled={page === 1}
              className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
            >
              Prev
            </button>
            <span className="self-center text-sm text-gray-600">
              Page {page} of {totalPages}
            </span>
            <button
              onClick={() => setPage((prev) => Math.min(prev + 1, totalPages))}
              disabled={page === totalPages}
              className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
            >
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
}

export default CompanyListPage;
