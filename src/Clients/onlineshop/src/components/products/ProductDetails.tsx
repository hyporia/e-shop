import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { productServiceApplicationEndpointsProductEndpointsGetProductByIdEndpoint } from "../../clients/ProductService";
import { ProductServiceContractsQueriesProductGetProductByIdResponse } from "../../clients/ProductService/types.gen";

type Product = {
    image: string;
} & ProductServiceContractsQueriesProductGetProductByIdResponse;

const ProductDetails = () => {
    const { id } = useParams<{ id: string }>();
    const [product, setProduct] = useState<Product | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchProduct = async () => {
            if (!id) return;

            try {
                setLoading(true);
                const response =
                    await productServiceApplicationEndpointsProductEndpointsGetProductByIdEndpoint(
                        {
                            path: { id },
                        }
                    );

                if (response.status === 200 && response.data) {
                    setProduct({
                        ...response.data,
                        image: "https://picsum.photos/600", // Larger image for details page
                    });
                } else {
                    setError("Product not found");
                }
            } catch (error) {
                console.error("Failed to fetch product:", error);
                setError("Failed to load product details");
            } finally {
                setLoading(false);
            }
        };

        fetchProduct();
    }, [id]);

    if (loading) return <div className="container p-lg">Loading...</div>;
    if (error) return <div className="container p-lg text-error">{error}</div>;
    if (!product)
        return <div className="container p-lg">Product not found</div>;

    return (
        <div className="container p-lg">
            <div className="grid grid-cols-2 gap-xl">
                <div className="rounded-lg overflow-hidden">
                    <img
                        src={product.image}
                        alt={product.name}
                        className="img-cover"
                    />
                </div>
                <div>
                    <h1 className="text-2xl font-bold mb-md">{product.name}</h1>
                    <p className="text-xl font-bold mb-lg">
                        ${product.price?.toFixed(2)}
                    </p>
                    {product.description && (
                        <p className="mb-xl text-gray-600">
                            {product.description}
                        </p>
                    )}
                    <button className="btn-primary">Add to Cart</button>
                </div>
            </div>
        </div>
    );
};

export default ProductDetails;
