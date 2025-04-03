import { useEffect, useState } from "react";
import { getProducts } from "../../clients/ProductService";
import { ProductResponseItem } from "../../clients/ProductService/types.gen";

type Product = {
    image: string;
} & ProductResponseItem;

const Products = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                setLoading(true);
                const productsResp = await getProducts();
                if (productsResp.status !== 200) {
                    throw new Error(
                        `Error fetching products: ${productsResp.error}`
                    );
                }
                const items = productsResp.data?.products || [];
                const products = items.map((item) => ({
                    ...item,
                    image: "https://picsum.photos/300",
                })); // Example image URL
                setProducts(products);
            } catch (error) {
                console.error("Failed to fetch products:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchProducts();
    }, []);

    return (
        <div className="container p-lg">
            <h1 className="text-3xl font-bold text-center mb-xl">
                Our Products
            </h1>
            {loading ? (
                <p className="text-center">Loading products...</p>
            ) : (
                <div className="grid grid-cols-products gap-lg">
                    {products.map((product) => (
                        <div key={product.id} className="card">
                            <div className="h-48 overflow-hidden">
                                <img
                                    src={product.image}
                                    alt={product.name}
                                    className="img-cover"
                                />
                            </div>
                            <div className="p-md">
                                <h3 className="text-lg font-semibold mb-sm">
                                    {product.name}
                                </h3>
                                <p className="text-primary font-bold mb-sm">
                                    ${product.price.toFixed(2)}
                                </p>
                                <p className="text-sm text-gray-600 line-clamp-3 mb-md">
                                    {product.description}
                                </p>
                                <button className="btn-primary">
                                    Add to Cart
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default Products;
